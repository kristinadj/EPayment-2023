using Base.DTO.Input;
using Base.DTO.Output;
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

        [HttpPost]
        public ActionResult CreateInvoice([FromBody] PaymentRequestIDTO paymentRequestDTO)
        {
            return Ok(new PaymentInstructionsODTO(paymentRequestDTO.TransactionSuccessUrl));
        }
    }
}