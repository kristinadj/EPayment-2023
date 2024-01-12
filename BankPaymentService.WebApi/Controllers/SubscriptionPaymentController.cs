using BankPaymentService.WebApi.Services;
using Base.DTO.Input;
using Base.Services.AppSettings;
using Base.Services.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BankPaymentService.WebApi.Controllers
{
    [Route("api/card/SubscriptionPayment")]
    [ApiController]
    public class SubscriptionPaymentController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IMerchantService _merchantService;
        private readonly IBankService _bankService;

        public SubscriptionPaymentController(
            IInvoiceService invoiceService,
            IMerchantService merchantService,
            IBankService bankService)
        {
            _invoiceService = invoiceService;
            _merchantService = merchantService;
            _bankService = bankService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateRecurringPayment([FromBody] RecurringPaymentRequestIDTO paymentRequestDTO)
        {
            var merchant = await _merchantService.GetMerchantByPaymentServiceMerchantId(paymentRequestDTO.MerchantId);
            if (merchant == null) return BadRequest();

            var invoice = await _invoiceService.CreateInvoiceAsync(paymentRequestDTO);
            if (invoice == null) return BadRequest();

            var paymentInstructions = await _bankService.SendRecurringPaymentToBankAsync(invoice, paymentRequestDTO);
            if (paymentInstructions == null) return BadRequest();

            return Ok(paymentInstructions);
        }

        [HttpPut("CancelSubscription/{subscriptionId}")]
        public async Task<ActionResult> CancelSubscription([FromRoute] string subscriptionId)
        {
            var isSuccess = int.TryParse(subscriptionId, out int recurringTransactionDefinitionId);
            if (!isSuccess) return NotFound();

            var merchant = await _merchantService.GetMerchantByBankRecurringTransactionId(recurringTransactionDefinitionId);
            if (merchant == null) return BadRequest();

            isSuccess = await _bankService.CancelSubscriptionAsync(recurringTransactionDefinitionId, merchant);
            if (!isSuccess) return BadRequest();

            return Ok();
        }
    }
}
