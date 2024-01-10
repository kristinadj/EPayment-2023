using Bank1.WebApi.AppSettings;
using Bank1.WebApi.DTO.Input;
using Bank1.WebApi.Services;
using Base.DTO.Output;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Bank1.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class RecurringTransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly BankSettings _appSettings;

        public RecurringTransactionController(
            ITransactionService transactionService,
            IAccountService accountService,
            INbsClient nbsClient,
            IOptions<BankSettings> appSettings)
        {
            _transactionService = transactionService;
            _appSettings = appSettings.Value;
        }

        [HttpPost]
        public async Task<ActionResult<PaymentInstructionsODTO>> CreateReccurringTransaction([FromBody] TransactionIDTO transactionIDTO)
        {
            try
            {
                var transaction = await _transactionService.CreateRecurringTransactionAsync(transactionIDTO);
                if (transaction == null) return BadRequest();

                var paymentUrl = $"{_appSettings.BankPaymentUrl}".Replace("@TRANSACTION_ID@", transaction.TransactionId.ToString());
                var paymentInstructions = new PaymentInstructionsODTO(transaction.TransactionId.ToString(), paymentUrl);
                return Ok(paymentInstructions);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Cancel/{recurringTransactionDefinitionId}")]
        public async Task<ActionResult> CancelRecurringTransaction([FromRoute] int recurringTransactionDefinitionId)
        {
            try
            {
                var isSuccess = await _transactionService.CancelRecurringTransactionAsync(recurringTransactionDefinitionId);
                if (!isSuccess) return BadRequest();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
