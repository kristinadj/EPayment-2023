using BankPaymentService.WebApi.AppSettings;
using BankPaymentService.WebApi.Services;
using Base.DTO.Input;
using Base.DTO.Output;
using Base.Services.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BankPaymentService.WebApi.Controllers
{
    [Route("api/qrcode/Invoice")]
    [ApiController]
    public class InvoiceQrCodeController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IBankService _bankService;

        private readonly CardPaymentMethod _cardPaymentMethod;
        private readonly IConsulHttpClient _consulHttpClient;

        public InvoiceQrCodeController(
            IOptions<CardPaymentMethod> cardPaymentMethod,
            IInvoiceService invoiceService,
            IBankService bankService,
            IConsulHttpClient consulHttpClient)
        {
            _cardPaymentMethod = cardPaymentMethod.Value;
            _invoiceService = invoiceService;
            _bankService = bankService;
            _consulHttpClient = consulHttpClient;
        }

        [HttpPost]
        public async Task<ActionResult<PaymentInstructionsODTO>> CreateInvoice([FromBody] PaymentRequestIDTO paymentRequestDTO)
        {
            var invoice = await _invoiceService.CreateInvoiceAsync(paymentRequestDTO);
            if (invoice == null) return BadRequest();

            var paymentInstructions = await _bankService.SendInvoiceToBankAsync(invoice, paymentRequestDTO, true);
            if (paymentInstructions == null) return BadRequest();

            return Ok(paymentInstructions);
        }
    }
}