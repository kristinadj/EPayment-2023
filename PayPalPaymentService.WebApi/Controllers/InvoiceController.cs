using Base.DTO.Shared;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace PayPalPaymentService.WebApi.Controllers
{
    [Route("api/paypal/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class InvoiceController : ControllerBase
    {
        public InvoiceController()
        {
        }

        [HttpGet]
        public ActionResult<RedirectUrlDTO> CreateInvoice()
        {
            var redirectUrl = new RedirectUrlDTO("Success PAYPAL");
            return Ok(redirectUrl);
        }
    }
}
