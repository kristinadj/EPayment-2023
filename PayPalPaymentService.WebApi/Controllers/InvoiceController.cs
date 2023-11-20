using Base.DTO.Input;
using Base.DTO.Output;
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
            return Ok(new PaymentInstructionsODTO(paymentRequestDTO.TransactionSuccessUrl));
        }
    }
}
