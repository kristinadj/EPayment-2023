using BankPaymentService.WebApi.AppSettings;
using BankPaymentService.WebApi.Services;
using Base.DTO.Input;
using Base.DTO.Output;
using Base.DTO.Shared;
using Base.Services.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BankPaymentService.WebApi.Controllers
{
    [Route("api/card/Invoice")]
    [ApiController]
    public class InvoiceCardController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IBankService _bankService;

        private readonly CardPaymentMethod _cardPaymentMethod;
        private readonly IConsulHttpClient _consulHttpClient;

        public InvoiceCardController(
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

            var paymentInstructions = await _bankService.SendInvoiceToBankAsync(invoice, paymentRequestDTO, false);
            if (paymentInstructions == null) return BadRequest();

            return Ok(paymentInstructions);
        }

        
    }
}
