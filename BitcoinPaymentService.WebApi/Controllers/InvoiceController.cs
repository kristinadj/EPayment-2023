using Base.DTO.Input;
using Base.DTO.Output;
using Base.Services.AppSettings;
using Base.Services.Clients;
using BitcoinPaymentService.WebApi.AppSettings;
using BitcoinPaymentService.WebApi.BitcoinClients;
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
        private readonly ICoingateClient _bitcoinClient;

        public InvoiceController(
            IInvoiceService invoiceService,
            IMerchantService merchantService,
            IOptions<PaymentMethod> paymetnMethod,
            IOptions<BitcoinSettings> bitcoinSettings,
            IConsulHttpClient consulHttpClient,
            ICoingateClient bitcoinClient)
        {
            _invoiceService = invoiceService;
            _merchantService = merchantService;
            _paymentMethod = paymetnMethod.Value;
            _bitcoinSettings = bitcoinSettings.Value;
            _consulHttpClient = consulHttpClient;
            _bitcoinClient = bitcoinClient;
        }

        [HttpPost]
        public async Task<ActionResult<PaymentInstructionsODTO>> CreateInvoice([FromBody] PaymentRequestIDTO paymentRequestDTO)
        {
            var merchant = await _merchantService.GetMerchantByPaymentServiceMerchantId(paymentRequestDTO.MerchantId);
            if (merchant == null) return BadRequest();

            var isPaid = await _invoiceService.IsInvoicePaidAsync(paymentRequestDTO.ExternalInvoiceId);
            if (isPaid) return BadRequest("Invoice already paid");

            var invoice = await _invoiceService.CreateInvoiceAsync(paymentRequestDTO);
            if (invoice == null) return BadRequest();

            var successUrl = _bitcoinSettings.SuccessUrl.Replace("@INVOICE_ID@", invoice.InvoiceId.ToString());
            var closeUrl = _bitcoinSettings.CancelUrl.Replace("@INVOICE_ID@", invoice.InvoiceId.ToString());

            var bitcointInvoice = await _bitcoinClient.CreateInvoiceAsync(merchant.ApiKey!,  invoice.InvoiceId, (decimal)invoice.Amount, invoice.Currency!.Code, successUrl, closeUrl);

            if (bitcointInvoice == null)
            {
                await _invoiceService.UpdateInvoiceStatusAsync(invoice.InvoiceId, Enums.TransactionStatus.ERROR);
                return BadRequest();
            }

            await _invoiceService.UpdateExternalPaymentServiceInvoiceIdAsync(invoice.InvoiceId, bitcointInvoice.Id.ToString());
            await _invoiceService.UpdateInvoiceStatusAsync(invoice.InvoiceId, Enums.TransactionStatus.IN_PROGRESS);

            return Ok(new PaymentInstructionsODTO(bitcointInvoice.Id.ToString(), bitcointInvoice.PaymentUrl));
        }

        [HttpGet("Coingate/Success/{invoiceId}")]
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

        [HttpGet("Coingate/Cancel/{invoiceId}")]
        public async Task<ActionResult> Cancel([FromRoute]int invoiceId)
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
