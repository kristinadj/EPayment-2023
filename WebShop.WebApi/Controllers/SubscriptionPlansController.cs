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

        private readonly WebShopAppSettings _webShopAppSettings;
        private readonly IPspApiHttpClient _pspApiHttpClient;

        private readonly IMapper _mapper;

        public SubscriptionPlansController(
            ISubscriptionPlanService subscriptionPlanService, 
            IInvoiceService invoiceService,
            IOptions<WebShopAppSettings> webShopAppSettings,
            IPspApiHttpClient pspApiHttpClient,
            IMapper mapper)
        {
            _subscriptionPlanService = subscriptionPlanService;
            _invoiceService = invoiceService;
            _webShopAppSettings = webShopAppSettings.Value;
            _pspApiHttpClient = pspApiHttpClient;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<SubscriptionPlanODTO>>> GetSubscriptionPlans()
        {
            try
            {
                var result = await _subscriptionPlanService.GetSubscriptionPlansAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("IsValid/{userId}")]
        public async Task<ActionResult<bool>> IsSubscriptionPlanValid([FromRoute] string userId)
        {
            try
            {
                var result = await _subscriptionPlanService.IsSubscriptionPlanValidAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Details/{userId}")]
        public async Task<ActionResult<UserSubscriptionPlanDetailsODTO>> SubscriptionPlanDetails([FromRoute] string userId)
        {
            try
            {
                var result = await _subscriptionPlanService.GetSubscriptionPlanDetailsAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Choose")]
        public async Task<ActionResult> ChooseSubscriptionPlanAsync([FromBody] UserSubscriptionPlanIDTO userSubscriptionPlanIDTO)
        {
            try
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
                    var result = await _pspApiHttpClient.PostAsync($"Invoice", pspPayment);

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

                    var result = await _pspApiHttpClient.PostAsync($"SubscriptionPayment", pspPayment);

                    if (result == null || string.IsNullOrEmpty(result.RedirectUrl))
                    {
                        return Ok(new RedirectUrlDTO($"/invoice/{invoice.InvoiceId}/error"));
                    }

                    return Ok(new RedirectUrlDTO(result!.RedirectUrl));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
            try
            {
                var userSubsriptionPlan = await _subscriptionPlanService.GetUserSubscriptionPlanByUserIdAsync(userId);
                if (userSubsriptionPlan == null) return NotFound($"Subscription Plan not found for the user {userId}");

                if (string.IsNullOrEmpty(userSubsriptionPlan.ExternalSubscriptionId)) return BadRequest("Invalid ExternalSubscriptionId");

                var paymentMethod = await _invoiceService.GetPaymentMethodByInvoiceIdAsync((int)userSubsriptionPlan.InvoiceId!);
                if (paymentMethod == null) return NotFound($"PaymentMethod ot found");

                var isSuccess = await _pspApiHttpClient.PutAsync($"SubscriptionPayment/CancelSubscription/{paymentMethod.PspPaymentMethodId}/{userSubsriptionPlan.ExternalSubscriptionId}");
                if (!isSuccess) return BadRequest("Unexpected exception while canceling subscription");

                await _subscriptionPlanService.CancelUserSubscriptionPlanAsync(userSubsriptionPlan);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Renewed/{userSubscriptionPlanId}/Success")]
        [AllowAnonymous]
        public async Task<ActionResult> UserSubscriptionPlanRenewedSuccessfully([FromRoute] int userSubscriptionPlanId)
        {
            try
            {
                await _subscriptionPlanService.UserSubscriptionPlanRenewalAsync(userSubscriptionPlanId, TransactionStatus.COMPLETED);
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
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
