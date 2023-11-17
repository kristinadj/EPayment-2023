using Base.DTO.Shared;
using Microsoft.AspNetCore.Cors;
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

        [HttpGet]
        public ActionResult<RedirectUrlDTO> CreateInvoice()
        {
            var redirectUrl = new RedirectUrlDTO("Success BITCOIN");
            return Ok(redirectUrl);
        }
    }
}
