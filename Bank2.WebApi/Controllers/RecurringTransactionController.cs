using Bank.DTO.Input;
using Bank2.WebApi.AppSettings;
using Bank2.WebApi.Services;
using Base.DTO.Output;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Bank2.WebApi.Controllers
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
                var paymentUrl = $"{_appSettings.BankPaymentUrl}".Replace("@TRANSACTION_ID@", transaction!.TransactionId.ToString());
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
                await _transactionService.CancelRecurringTransactionAsync(recurringTransactionDefinitionId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
