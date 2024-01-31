using BankPaymentService.WebApi.Services;
using Base.DTO.Input;
using Base.DTO.Output;
using Microsoft.AspNetCore.Mvc;

namespace BankPaymentService.WebApi.Controllers
{
    [Route("api/card/Invoice")]
    [ApiController]
    public class InvoiceCardController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IBankService _bankService;

        public InvoiceCardController(IInvoiceService invoiceService, IBankService bankService)
        {
            _invoiceService = invoiceService;
            _bankService = bankService;
        }

        [HttpPost]
        public async Task<ActionResult<PaymentInstructionsODTO>> CreateInvoice([FromBody] PaymentRequestIDTO paymentRequestDTO)
        {
            try
            {
                var isPaid = await _invoiceService.IsInvoicePaidAsync(paymentRequestDTO.ExternalInvoiceId);
                if (isPaid) return BadRequest("Invoice already paid");

                var invoice = await _invoiceService.CreateInvoiceAsync(paymentRequestDTO);
                if (invoice == null) return BadRequest();

                var paymentInstructions = await _bankService.SendInvoiceToBankAsync(invoice, paymentRequestDTO, false);
                if (paymentInstructions == null) return BadRequest();

                return Ok(paymentInstructions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
