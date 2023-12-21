using Base.DTO.Input;
using Base.DTO.Output;
using Base.Services.AppSettings;
using Base.Services.Clients;
using BitcoinPaymentService.WebApi.AppSettings;
using BitcoinPaymentService.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BitcoinPaymentService.WebApi.Controllers
{
    [Route("api/bitcoin/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IMerchantService _merchantService;
        private readonly BitcoinSettings _bitcoinSettings;

        private readonly PaymentMethod _paymentMethod;
        private readonly IConsulHttpClient _consulHttpClient;

        public InvoiceController(
            IInvoiceService invoiceService,
            IMerchantService merchantService,
            IOptions<PaymentMethod> paymetnMethod,
            IOptions<BitcoinSettings> bitcoinSettings,
            IConsulHttpClient consulHttpClient)
        {
            _invoiceService = invoiceService;
            _merchantService = merchantService;
            _paymentMethod = paymetnMethod.Value;
            _bitcoinSettings = bitcoinSettings.Value;
            _consulHttpClient = consulHttpClient;
        }

        [HttpPost]
        public async Task<ActionResult<PaymentInstructionsODTO>> CreateInvoice([FromBody] PaymentRequestIDTO paymentRequestDTO)
        {
            var merchant = await _merchantService.GetMerchantByPaymentServiceMerchantId(paymentRequestDTO.MerchantId);
            if (merchant == null) return BadRequest();


            var invoice = await _invoiceService.CreateInvoiceAsync(paymentRequestDTO);
            if (invoice == null) return BadRequest();

            var successUrl = _bitcoinSettings.RedirectUrl.Replace("@INVOICE_ID@", invoice.InvoiceId.ToString());
            var closeUrl = _bitcoinSettings.RedirectUrl.Replace("@INVOICE_ID@", invoice.InvoiceId.ToString());
            var bitPayInvoice = await BitcoinClient.CreateInvoiceAsync(merchant.Token, invoice.InvoiceId, (decimal)invoice.Amount, invoice.Currency!.Code, successUrl, closeUrl);

            if (bitPayInvoice == null)
            {
                await _invoiceService.UpdateInvoiceStatusAsync(invoice.InvoiceId, Enums.TransactionStatus.ERROR);
                return BadRequest();
            }

            await _invoiceService.UpdateBitPayIdAsync(invoice.InvoiceId, bitPayInvoice.Id!);
            await _invoiceService.UpdateInvoiceStatusAsync(invoice.InvoiceId, Enums.TransactionStatus.IN_PROGRESS);

            return Ok(new PaymentInstructionsODTO(bitPayInvoice.Url!));
        }

        [HttpGet("BitPay/Success/{invoiceId}")]
        public async Task<ActionResult> Success([FromRoute] int invoiceId)
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(invoiceId);
            if (invoice == null) return BadRequest();

            await _invoiceService.UpdateInvoiceStatusAsync(invoice.InvoiceId, Enums.TransactionStatus.COMPLETED);
            try
            {
                await _consulHttpClient.PutAsync(_paymentMethod.PspServiceName, $"{invoice!.ExternalInvoiceId}/Success");
                return Redirect(invoice.TransactionSuccessUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("BitPay/Cancel/{invoiceId}")]
        public async Task<ActionResult> Cancel([FromRoute] int invoiceId)
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(invoiceId);
            if (invoice == null) return BadRequest();

            await _invoiceService.UpdateInvoiceStatusAsync(invoice.InvoiceId, Enums.TransactionStatus.FAIL);
            try
            {
                await _consulHttpClient.PutAsync(_paymentMethod.PspServiceName, $"{invoice!.ExternalInvoiceId}/Failure");
                return Redirect(invoice.TransactionFailureUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
