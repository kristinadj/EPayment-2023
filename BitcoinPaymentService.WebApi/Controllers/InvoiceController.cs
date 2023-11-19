using Base.DTO.Input;
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
            return Ok();
        }
    }
}
