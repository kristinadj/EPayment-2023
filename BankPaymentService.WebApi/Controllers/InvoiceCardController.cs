using BankPaymentService.WebApi.AppSettings;
using BankPaymentService.WebApi.Services;
using Base.DTO.Input;
using Base.DTO.Output;
using Base.DTO.Shared;
using Base.Services.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BankPaymentService.WebApi.Controllers
{
    [Route("api/card/Invoice")]
    [ApiController]
    public class InvoiceCardController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IBankService _bankService;

        private readonly CardPaymentMethod _cardPaymentMethod;
        private readonly IConsulHttpClient _consulHttpClient;

        public InvoiceCardController(
            IOptions<CardPaymentMethod> cardPaymentMethod,
            IInvoiceService invoiceService, 
            IBankService bankService, 
            IConsulHttpClient consulHttpClient)
        {
            _cardPaymentMethod = cardPaymentMethod.Value;
            _invoiceService = invoiceService;
            _bankService = bankService;
            _consulHttpClient = consulHttpClient;
        }

        [HttpPost]
        public async Task<ActionResult<PaymentInstructionsODTO>> CreateInvoice([FromBody] PaymentRequestIDTO paymentRequestDTO)
        {
            var invoice = await _invoiceService.CreateInvoiceAsync(paymentRequestDTO);
            if (invoice == null) return BadRequest();

            var paymentInstructions = await _bankService.SendInvoiceToBankAsync(invoice, paymentRequestDTO);
            if (paymentInstructions == null) return BadRequest();

            return Ok(paymentInstructions);
        }

        [HttpPut("{invoiceId}/Success")]
        public async Task<ActionResult<RedirectUrlDTO>> SuccessPayment([FromRoute] int invoiceId)
        {
            var invoice = await _invoiceService.UpdateInvoiceStatusAsync(invoiceId, Enums.TransactionStatus.COMPLETED);
            if (invoice == null) return BadRequest();

            try
            {
                await _consulHttpClient.PutAsync(_cardPaymentMethod.PspServiceName, $"{invoice.ExternalInvoiceId}/Success");
                var redirectUrl = new RedirectUrlDTO(invoice.TransactionSuccessUrl);
                return Ok(redirectUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{invoiceId}/Failure")]
        public async Task<ActionResult<RedirectUrlDTO>> FailurePayment([FromRoute] int invoiceId)
        {
            var invoice = await _invoiceService.UpdateInvoiceStatusAsync(invoiceId, Enums.TransactionStatus.FAIL);
            if (invoice == null) return BadRequest();

            try
            {
                await _consulHttpClient.PutAsync(_cardPaymentMethod.PspServiceName, $"{invoice.ExternalInvoiceId}/Failure");
                var redirectUrl = new RedirectUrlDTO(invoice.TransactionFailureUrl);
                return Ok(redirectUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{invoiceId}/Error")]
        public async Task<ActionResult<RedirectUrlDTO>> ErrorPayment([FromRoute] int invoiceId)
        {
            var invoice = await _invoiceService.UpdateInvoiceStatusAsync(invoiceId, Enums.TransactionStatus.ERROR);
            if (invoice == null) return BadRequest();

            try
            {
                await _consulHttpClient.PutAsync(_cardPaymentMethod.PspServiceName, $"{invoice.ExternalInvoiceId}/Error");
                var redirectUrl = new RedirectUrlDTO(invoice.TransactionErrorUrl);
                return Ok(redirectUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
