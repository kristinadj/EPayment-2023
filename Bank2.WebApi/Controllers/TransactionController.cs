using Bank2.WebApi.AppSettings;
using Bank2.WebApi.DTO.Input;
using Bank2.WebApi.Services;
using Base.DTO.Input;
using Base.DTO.Output;
using Base.DTO.Shared;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Bank2.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IAccountService _accountService;
        private readonly BankSettings _appSettings;

        public TransactionController(ITransactionService transactionService, IAccountService accountService, IOptions<BankSettings> appSettings)
        {
            _transactionService = transactionService;
            _accountService = accountService;
            _appSettings = appSettings.Value;
        }

        [HttpPost]
        public async Task<ActionResult<PaymentInstructionsODTO>> CreateTransaction([FromBody] TransactionIDTO transactionIDTO)
        {
            try
            {
                var transaction = await _transactionService.CreateTransactionAsync(transactionIDTO);
                if (transaction == null) return BadRequest();

                var paymentInstructions = new PaymentInstructionsODTO(_appSettings.BankPaymentUrl.Replace("@TRANSACTION_ID@", transaction.TransactionId.ToString()))
                {
                    PaymentId = transaction.TransactionId
                };
                return Ok(paymentInstructions);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> PayTransaction([FromBody] PayTransactionIDTO payTransactionIDTO)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(payTransactionIDTO.TransactionId);
            if (transaction == null) return NotFound();

            if (transaction.TransactionLogs!.Any(x => x.TransactionStatus == Enums.TransactionStatus.COMPLETED))
                return BadRequest("Transaction already paid");

            RedirectUrlDTO? redirectUrl = null;
            try
            {
                // TODO: VERIFY CARD
                var isLocalCard = payTransactionIDTO.PanNumber.StartsWith(_appSettings.CardStartNumbers);

                if (!isLocalCard)
                {
                    // TODO: PCC
                }
                else
                {
                    var sender = await _accountService.GetAccountByCreditCardAsync(payTransactionIDTO);
                    var isSuccess = await _transactionService.PayTransctionAsync(transaction, sender!);
                    if (isSuccess)
                    {
                        redirectUrl = await _transactionService.UpdatePaymentServiceInvoiceStatusAsync(transaction.TransactionSuccessUrl);
                    }
                    else
                    {
                        redirectUrl = await _transactionService.UpdatePaymentServiceInvoiceStatusAsync(transaction.TransactionFailureUrl);
                    }
                }
            }
            catch (Exception)
            {
                redirectUrl = await _transactionService.UpdatePaymentServiceInvoiceStatusAsync(transaction.TransactionErrorUrl);
            }

            return Ok(redirectUrl);
        }

    }
}
