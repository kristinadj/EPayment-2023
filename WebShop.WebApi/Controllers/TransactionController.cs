using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebShop.DTO.Enums;
using WebShop.WebApi.Services;

namespace WebShop.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPut("{transactionId};{transactionStatus}")]
        public async Task<ActionResult> UpdateTransactionStatus([FromRoute] int transactionId, [FromRoute] TransactionStatus transactionStatus)
        {
            try
            {
                var isSuccess = await _transactionService.UpdateTransactionStatusAsync(transactionId, transactionStatus);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
