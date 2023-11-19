using Base.DTO.Input;
using Microsoft.AspNetCore.Mvc;

namespace PayPalPaymentService.WebApi.Controllers
{
    [Route("api/paypal/[controller]")]
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
