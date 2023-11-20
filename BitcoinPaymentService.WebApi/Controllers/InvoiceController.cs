using Base.DTO.Input;
using Base.DTO.Output;
using Microsoft.AspNetCore.Mvc;

namespace BitcoinPaymentService.WebApi.Controllers
{
    [Route("api/bitcoin/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        public InvoiceController()
        {
        }

        [HttpPost]
        public ActionResult CreateInvoice([FromBody] PaymentRequestIDTO paymentRequestDTO)
        {
            return Ok(new PaymentInstructionsODTO(paymentRequestDTO.TransactionSuccessUrl));
        }
    }
}
