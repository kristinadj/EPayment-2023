using Microsoft.AspNetCore.Cors;
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

        [HttpGet]
        public ActionResult CreateInvoice()
        {
            return Ok();
        }
    }
}
