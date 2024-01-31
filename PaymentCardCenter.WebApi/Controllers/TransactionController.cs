using Base.DTO.Input;
using Base.DTO.Output;
using Microsoft.AspNetCore.Mvc;
using PaymentCardCenter.WebApi.Services;

namespace PaymentCardCenter.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("ReceiveAcquirerTransaction")]
        public async Task<ActionResult<PccAquirerTransactionODTO>> ResolvePayment([FromBody] PccAquirerTransactionIDTO pccTransactionIDTO)
        {
            try
            {
                var transactionODTO = await _transactionService.ResolvePaymentAsync(pccTransactionIDTO);
                if (transactionODTO == null) return BadRequest();
                return Ok(transactionODTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ReceiveIssuerTransaction")]
        public async Task<ActionResult<PccAquirerTransactionODTO>> ResolvePayment([FromBody] PccIssuerTransactionIDTO pccTransactionIDTO)
        {
            try
            {
                var transactionODTO = await _transactionService.ResolvePaymentAsync(pccTransactionIDTO);
                if (transactionODTO == null) return BadRequest();
                return Ok(transactionODTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
