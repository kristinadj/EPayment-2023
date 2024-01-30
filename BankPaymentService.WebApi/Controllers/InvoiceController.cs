using BankPaymentService.WebApi.AppSettings;
using BankPaymentService.WebApi.Services;
using Base.DTO.Shared;
using Base.Services.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BankPaymentService.WebApi.Controllers
{
    [Route("api/Invoice")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        private readonly CardPaymentMethod _cardPaymentMethod;
        private readonly IConsulHttpClient _consulHttpClient;

        public InvoiceController(
            IOptions<CardPaymentMethod> cardPaymentMethod,
            IInvoiceService invoiceService,
            IConsulHttpClient consulHttpClient)
        {
            _cardPaymentMethod = cardPaymentMethod.Value;
            _invoiceService = invoiceService;
            _consulHttpClient = consulHttpClient;
        }

        [HttpPut("{invoiceId}/Success")]
        public async Task<ActionResult<RedirectUrlDTO>> SuccessPayment([FromRoute] int invoiceId)
        {
            var invoice = await _invoiceService.GetInvoiceByInvoiceIdAsync(invoiceId);
            if (invoice == null) return NotFound($"Invoice {invoiceId} not found");

            if (invoice.TransactionStatus == Enums.TransactionStatus.COMPLETED)
            {
                var redirectUrl = new RedirectUrlDTO(invoice.TransactionSuccessUrl);
                return Ok(redirectUrl);
            }

            invoice = await _invoiceService.UpdateInvoiceStatusAsync(invoiceId, Enums.TransactionStatus.COMPLETED);
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

        [HttpPut("{invoiceId}/Success/{recurringTransactionId}")]
        public async Task<ActionResult<RedirectUrlDTO>> SuccessPaymentSubscription([FromRoute] int invoiceId, [FromRoute] int recurringTransactionId)
        {
            var invoice = await _invoiceService.UpdateInvoiceStatusAsync(invoiceId, Enums.TransactionStatus.COMPLETED);
            if (invoice == null) return BadRequest();

            await _invoiceService.UpdateBankRecurringTransactionId(invoice.InvoiceId, recurringTransactionId);

            try
            {
                await _consulHttpClient.PutAsync(_cardPaymentMethod.PspServiceName, $"{invoice.ExternalInvoiceId}/Success");
                var redirectUrl = new RedirectUrlDTO($"{invoice.TransactionSuccessUrl}/{recurringTransactionId}");
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
            var invoice = await _invoiceService.GetInvoiceByInvoiceIdAsync(invoiceId);
            if (invoice == null) return NotFound($"Invoice {invoiceId} not found");

            if (invoice.TransactionStatus == Enums.TransactionStatus.FAIL)
            {
                var redirectUrl = new RedirectUrlDTO(invoice.TransactionFailureUrl);
                return Ok(redirectUrl);
            }

            invoice = await _invoiceService.UpdateInvoiceStatusAsync(invoiceId, Enums.TransactionStatus.FAIL);
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
            var invoice = await _invoiceService.GetInvoiceByInvoiceIdAsync(invoiceId);
            if (invoice == null) return NotFound($"Invoice {invoiceId} not found");

            if (invoice.TransactionStatus == Enums.TransactionStatus.ERROR)
            {
                var redirectUrl = new RedirectUrlDTO(invoice.TransactionErrorUrl);
                return Ok(redirectUrl);
            }

            invoice = await _invoiceService.UpdateInvoiceStatusAsync(invoiceId, Enums.TransactionStatus.ERROR);
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
