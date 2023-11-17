using Base.DTO.Shared;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BankPaymentService.WebApi.Controllers
{
    [Route("api/card/Invoice")]
    [ApiController]
    public class InvoiceCardController : ControllerBase
    {
        public InvoiceCardController()
        {
        }

        [HttpGet]
        public ActionResult CreateInvoice()
        {
            return Ok();
        }
    }
}
