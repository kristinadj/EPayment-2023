using Base.DTO.Shared;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BankPaymentService.WebApi.Controllers
{
    [Route("api/card/Invoice")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class InvoiceCardController : ControllerBase
    {
        public InvoiceCardController()
        {
        }

        [HttpGet]
        public ActionResult<RedirectUrlDTO> CreateInvoice()
        {
            var redirectUrl = new RedirectUrlDTO("Success CARD");
            return Ok(redirectUrl);
        }
    }
}
