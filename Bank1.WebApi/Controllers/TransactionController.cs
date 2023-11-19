using Bank1.WebApi.AppSettings;
using Bank1.WebApi.DTO.Input;
using Bank1.WebApi.Models;
using Bank1.WebApi.Services;
using Base.DTO.Output;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Bank1.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly UrlAppSettings _urlAppSettings;
        public TransactionController(ITransactionService transactionService, IOptions<UrlAppSettings> urlAppSettings)
        {
            _transactionService = transactionService;
            _urlAppSettings = urlAppSettings.Value;
        }

        [HttpPost]
        public async Task<ActionResult<PaymentInstructionsODTO>> CreateTransaction([FromBody] TransactionIDTO transactionIDTO)
        {
            try
            {
                var transaction = await _transactionService.CreateTransactionAsync(transactionIDTO);
                if (transaction == null) return BadRequest();

                var paymentInstructions = new PaymentInstructionsODTO(_urlAppSettings.BankPaymentUrl.Replace("@TRANSACTION_ID@", transaction.TransactionId.ToString()))
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

    }
}
