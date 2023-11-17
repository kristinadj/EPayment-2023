using Base.DTO.Shared;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BankPaymentService.WebApi.Controllers
{
    [Route("api/qrcode/Invoice")]
    [ApiController]
    public class InvoiceQrCodeController : ControllerBase
    {
        public InvoiceQrCodeController()
        {
        }

        [HttpGet]
        public ActionResult CreateInvoice()
        {
            return Ok();
        }
    }
}