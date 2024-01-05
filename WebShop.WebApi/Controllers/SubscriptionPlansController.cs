using AutoMapper;
using Base.DTO.Shared;
using Base.Services.Clients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebShop.DTO.Enums;
using WebShop.DTO.Input;
using WebShop.DTO.Output;
using WebShop.WebApi.AppSettings;
using WebShop.WebApi.Services;

namespace WebShop.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    [Authorize]
    public class SubscriptionPlansController : ControllerBase
    {
        private readonly ISubscriptionPlanService _subscriptionPlanService;
        private readonly IInvoiceService _invoiceService;

        private readonly PspAppSettings _pspAppSettings;
        private readonly IConsulHttpClient _consulHttpClient;

        private readonly IMapper _mapper;

        public SubscriptionPlansController(
            ISubscriptionPlanService subscriptionPlanService, 
            IInvoiceService invoiceService,
            IOptions<PspAppSettings> pspAppSettings,
            IConsulHttpClient consulHttpClient,
            IMapper mapper)
        {
            _subscriptionPlanService = subscriptionPlanService;
            _invoiceService = invoiceService;
            _pspAppSettings = pspAppSettings.Value;
            _consulHttpClient = consulHttpClient;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<SubscriptionPlanODTO>>> GetSubscriptionPlans()
        {
            var result = await _subscriptionPlanService.GetSubscriptionPlansAsync();
            return Ok(result);
        }

        [HttpGet("IsValid/{userId}")]
        public async Task<ActionResult<bool>> IsSubscriptionPlanValid([FromRoute] string userId)
        {
            var result = await _subscriptionPlanService.IsSubscriptionPlanValidAsync(userId);
            return Ok(result);
        }

        [HttpGet("Details/{userId}")]
        public async Task<ActionResult<SubscriptionPlanDetailsODTO>> SubscriptionPlanDetails([FromRoute] string userId)
        {
            var result = await _subscriptionPlanService.GetSubscriptionPlanDetailsAsync(userId);
            return Ok(result);
        }

        [HttpPost("Choose")]
        public async Task<ActionResult> ChooseSubscriptionPlanAsync([FromBody] UserSubscriptionPlanIDTO userSubscriptionPlanIDTO)
        {
            var userSubscriptionPlan = await _subscriptionPlanService.AddUserSubscriptionPlanAsync(userSubscriptionPlanIDTO);
            if (userSubscriptionPlan == null) return NotFound();

            var invoice = await _invoiceService.CreateInvoiceForSubscriptionPlanAsync(userSubscriptionPlan);
            if (invoice == null) return BadRequest();

            await _invoiceService.UpdateInvoiceTransactionStatusasync(invoice.InvoiceId, TransactionStatus.IN_PROGRESS);

            var pspPayment = _mapper.Map<PspInvoiceIDTO>(invoice);
            var result = await _consulHttpClient.PostAsync(_pspAppSettings.ServiceName, $"/api/Invoice", pspPayment);

            if (result == null || string.IsNullOrEmpty(result.RedirectUrl))
            {
                return Ok(new RedirectUrlDTO($"/invoice/{invoice.InvoiceId}/error"));
            }

            return Ok(new RedirectUrlDTO(result!.RedirectUrl));
        }
    }
}
