using AutoMapper;
using Base.DTO.Shared;
using Base.Services.Clients;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PSP.WebApi.AppSettings;
using PSP.WebApi.DTO.Output;
using PSP.WebApi.Services;

namespace PSP.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class SubscriptionPaymentController : ControllerBase
    {
        private readonly IPaymentMethodService _paymentMethodService;
        private readonly IMerchantService _merchantService;
        private readonly IInvoiceService _invoiceService;
        private readonly IConsulHttpClient _consulHttpClient;

        private readonly IMapper _mapper;
        private readonly PspAppSettings _pspAppSettings;

        public SubscriptionPaymentController(
            IPaymentMethodService paymentMethodService,
            IMerchantService merchantService,
            IInvoiceService invoiceService,
            IConsulHttpClient consulHttpClient,
            IMapper mapper,
            IOptions<PspAppSettings> pspAppSettings)
        {
            _paymentMethodService = paymentMethodService;
            _merchantService = merchantService;
            _invoiceService = invoiceService;
            _consulHttpClient = consulHttpClient;
            _mapper = mapper;
            _pspAppSettings = pspAppSettings.Value;
        }

        [HttpPost]
        public async Task<ActionResult<string>> CreateSubscriptionPayment([FromBody] PspSubscriptionPaymentDTO subscriptionPaymentDTO)
        {
            try
            {
                var merchant = await _merchantService.GetMerchantByIdAsync(subscriptionPaymentDTO.MerchantId);
                var invoice = await _invoiceService.CreateInvoiceAsync(merchant!, subscriptionPaymentDTO);

                var result = _mapper.Map<InvoiceODTO>(subscriptionPaymentDTO);
                result.RedirectUrl = $"{_pspAppSettings.ClientUrl}/paymentMethods/{invoice!.InvoiceId}/true";
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("CancelSubscription/{paymentMethodId}/{subscriptionId}")]
        public async Task<ActionResult> CancelSubscription([FromRoute] int paymentMethodId, [FromRoute] string subscriptionId)
        {
            try
            {
                var paymentMethod = await _paymentMethodService.GetPaymentMethodByIdAsync(paymentMethodId);
                var isSuccess = await _consulHttpClient.PutAsync(paymentMethod!.ServiceName, $"{paymentMethod!.ServiceApiSufix}/SubscriptionPayment/CancelSubscription/{subscriptionId}");

                if (!isSuccess) return BadRequest("Unexpected error while canceling subscription");

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
