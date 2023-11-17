using AutoMapper;
using Base.Services.AppSettings;
using Base.Services.Clients;
using Base.DTO.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebShop.DTO.Enums;
using WebShop.DTO.Output;
using WebShop.DTO.PSP;
using WebShop.WebApi.AppSettings;
using WebShop.WebApi.Services;

namespace WebShop.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    [Authorize]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IPaymentMethodService _paymentMethodService;

        private readonly PspAppSettings _pspAppSettings;
        private readonly IConsulHttpClient _consulHttpClient;

        private readonly IMapper _mapper;

        public InvoiceController(
            IInvoiceService invoiceService, 
            IPaymentMethodService paymentMethodService,
            IOptions<PspAppSettings> pspAppSettings,
            IConsulHttpClient consulHttpClient,
            IMapper mapper)
        {
            _invoiceService = invoiceService;
            _paymentMethodService = paymentMethodService;
            _pspAppSettings = pspAppSettings.Value;
            _consulHttpClient = consulHttpClient;
            _mapper = mapper;
        }

        [HttpPost("{orderId};{paymentMethodId}")]
        public async Task<ActionResult<RedirectUrlDTO>> CreateInvoice([FromRoute] int orderId, [FromRoute] int paymentMethodId)
        {
            var paymentMethod = await _paymentMethodService.GetPaymentMethodById(paymentMethodId);
            if (paymentMethod == null) return NotFound();

            var invoice = await _invoiceService.CreateInvoiceAsync(orderId, paymentMethodId);

            if (invoice == null) return BadRequest();

            await _invoiceService.UpdateInvoiceTransactionStatusasync(invoice.InvoiceId, TransactionStatus.IN_PROGRESS);

            var pspPayment = _mapper.Map<PspPaymentIDTO>(invoice);
            var result = await _consulHttpClient.PostAsync(_pspAppSettings.ServiceName, $"/api/Invoice/{paymentMethod.PspPaymentMethodId}", pspPayment);

            if (result == null || string.IsNullOrEmpty(result.RedirectUrl))
            {
                await _invoiceService.UpdateInvoiceTransactionStatusasync(invoice.InvoiceId, TransactionStatus.ERROR);
                return BadRequest();
            }
            
            return Ok(new RedirectUrlDTO(result!.RedirectUrl));
        }
    }
}
