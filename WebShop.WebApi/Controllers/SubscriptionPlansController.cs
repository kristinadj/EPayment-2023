using AutoMapper;
using Base.DTO.Shared;
using Base.Services.Clients;
using Base.DTO.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebShop.DTO.Enums;
using WebShop.DTO.Input;
using WebShop.DTO.Output;
using WebShop.WebApi.AppSettings;
using WebShop.WebApi.Services;
using System.Web;

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
        private readonly WebShopAppSettings _webShopAppSettings;
        private readonly IConsulHttpClient _consulHttpClient;

        private readonly IMapper _mapper;

        public SubscriptionPlansController(
            ISubscriptionPlanService subscriptionPlanService, 
            IInvoiceService invoiceService,
            IOptions<PspAppSettings> pspAppSettings,
             IOptions<WebShopAppSettings> webShopAppSettings,
            IConsulHttpClient consulHttpClient,
            IMapper mapper)
        {
            _subscriptionPlanService = subscriptionPlanService;
            _invoiceService = invoiceService;
            _pspAppSettings = pspAppSettings.Value;
            _webShopAppSettings = webShopAppSettings.Value;
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
        public async Task<ActionResult<UserSubscriptionPlanDetailsODTO>> SubscriptionPlanDetails([FromRoute] string userId)
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

            if (!userSubscriptionPlan.SubscriptionPlan!.AutomaticRenewel)
            {
                var pspPayment = _mapper.Map<PspInvoiceIDTO>(invoice);
                pspPayment.InvoiceType = InvoiceType.SUBSCRIPTION;
                var result = await _consulHttpClient.PostAsync(_pspAppSettings.ServiceName, $"/api/Invoice", pspPayment);

                if (result == null || string.IsNullOrEmpty(result.RedirectUrl))
                {
                    return Ok(new RedirectUrlDTO($"/invoice/{invoice.InvoiceId}/error"));
                }

                return Ok(new RedirectUrlDTO(result!.RedirectUrl));
            }
            else
            {
                var pspPayment = _mapper.Map<PspSubscriptionPaymentDTO>(invoice);
                pspPayment.Subscriber = new Base.DTO.Input.SubscriberIDTO
                {
                    Name = invoice.User!.Name,
                    Email = invoice.User!.Email
                };
                pspPayment.BrandName = invoice.Merchant!.User!.Name;
                pspPayment.Product = new Base.DTO.Input.ProductIDTO("Yearly subscription", "SERVICE", string.Empty, "LEGAL");
                pspPayment.RecurringTransactionSuccessUrl = $"{_webShopAppSettings.WebApiUrl}/SubscriptionPlans/Renewed/{userSubscriptionPlan.UserSubscriptionPlanId}/Success";
                pspPayment.RecurringTransactionFailureUrl = $"{_webShopAppSettings.WebApiUrl}/SubscriptionPlans/Renewed/{userSubscriptionPlan.UserSubscriptionPlanId}/Failure";

                var result = await _consulHttpClient.PostAsync(_pspAppSettings.ServiceName, $"/api/SubscriptionPayment", pspPayment);

                if (result == null || string.IsNullOrEmpty(result.RedirectUrl))
                {
                    return Ok(new RedirectUrlDTO($"/invoice/{invoice.InvoiceId}/error"));
                }

                return Ok(new RedirectUrlDTO(result!.RedirectUrl));
            }
        }

        [HttpPut("ExternalSubscriptionId")]
        public async Task<ActionResult> UpdateExternalSubscriptionId([FromQuery] int invoiceId, [FromQuery] string externalSubscriptionId)
        {
            try
            {
                var isSuccess = await _subscriptionPlanService.UpdateExternalSubscriptionIdAsync(invoiceId, externalSubscriptionId);
                if (!isSuccess) return NotFound();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("CancelSubscription/{userId}")]
        [AllowAnonymous]
        public async Task<ActionResult> CancelSubscriptionAsync([FromRoute] string userId)
        {
            var userSubsriptionPlan = await _subscriptionPlanService.GetUserSubscriptionPlanByUserIdAsync(userId);
            if (userSubsriptionPlan == null) return NotFound();

            if (string.IsNullOrEmpty(userSubsriptionPlan.ExternalSubscriptionId)) return BadRequest();

            var paymentMethod = await _invoiceService.GetPaymentMethodByInvoiceIdAsync((int)userSubsriptionPlan.InvoiceId!);
            if (paymentMethod == null) return NotFound();

            var isSuccess = await _consulHttpClient.PutAsync(_pspAppSettings.ServiceName, $"/api/SubscriptionPayment/CancelSubscription/{paymentMethod.PspPaymentMethodId}/{userSubsriptionPlan.ExternalSubscriptionId}");
            if (!isSuccess) return BadRequest();

            await _subscriptionPlanService.CancelUserSubscriptionPlanAsync(userSubsriptionPlan);
            return Ok();
        }

        [HttpPost("Renewed/{userSubscriptionPlanId}/Success")]
        [AllowAnonymous]
        public async Task<ActionResult> UserSubscriptionPlanRenewedSuccessfully([FromRoute] int userSubscriptionPlanId)
        {
            try
            {
                var isSuccess = await _subscriptionPlanService.UserSubscriptionPlanRenewalAsync(userSubscriptionPlanId, TransactionStatus.COMPLETED);
                if (!isSuccess) return BadRequest();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Renewed/{userSubscriptionPlanId}/Failure")]
        public async Task<ActionResult> UserSubscriptionPlanRenewalFailed([FromRoute] int userSubscriptionPlanId)
        {
            try
            {
                var isSuccess = await _subscriptionPlanService.UserSubscriptionPlanRenewalAsync(userSubscriptionPlanId, TransactionStatus.FAIL);
                if (!isSuccess) return BadRequest();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
