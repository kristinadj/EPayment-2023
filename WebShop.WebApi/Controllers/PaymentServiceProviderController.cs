using Base.DTO.Output;
using Base.DTO.Shared;
using Base.Services.AppSettings;
using Base.Services.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebShop.DTO.Input;
using WebShop.DTO.Output;
using WebShop.WebApi.AppSettings;
using WebShop.WebApi.Services;

namespace WebShop.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentServiceProviderController : ControllerBase
    {
        private readonly WebShopAppSettings _webShopAppSettings;
        private readonly ConsulAppSettings _consulAppSettings;

        private readonly IMerchantService _merchantService;
        private readonly IPaymentMethodService _paymentMethodService;
        private readonly IPspApiHttpClient _pspApiHttpClient;

        public PaymentServiceProviderController(
            IMerchantService merchantService,
            IPaymentMethodService paymentMethodService,
            IOptions<ConsulAppSettings> consulAppSettings,
            IOptions<WebShopAppSettings> webShopAppSettings,
            IPspApiHttpClient pspApiHttpClient)
        {
            _merchantService = merchantService;
            _paymentMethodService = paymentMethodService;
            _consulAppSettings = consulAppSettings.Value;
            _webShopAppSettings = webShopAppSettings.Value;
            _pspApiHttpClient = pspApiHttpClient;
        }

        [HttpGet("Merchant/IsRegistered/{userId}")]
        public async Task<ActionResult<MerchantDTO>> IsMerchantRegistered([FromRoute] string userId)
        {
            try
            {
                var merchant = await _merchantService.GetMerchantByUserIdAsync(userId);
                if (merchant == null || merchant.PspMerchantId == null) return NotFound("Merchant not found");

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Merchant/Register/{userId}")]
        public async Task<ActionResult<MerchantDTO>> Register([FromRoute] string userId)
        {
            try
            {
                var merchant = await _merchantService.GetMerchantByUserIdAsync(userId);
                if (merchant == null) return NotFound("Merchant not found");

                var merchantDTO = new MerchantDTO(merchant.MerchantId.ToString(), merchant.User!.Name, merchant.User!.Address!, merchant.User!.PhoneNumber!, merchant.User!.Email!, _consulAppSettings.Service)
                {
                    TransactionSuccessUrl = $"{_webShopAppSettings.ClientUrl}/invoice/@INVOICE_ID@/success",
                    TransactionFailureUrl = $"{_webShopAppSettings.ClientUrl}/invoice/@INVOICE_ID@/failure",
                    TransactionErrorUrl = $"{_webShopAppSettings.ClientUrl}/invoice/@INVOICE_ID@/error"
                };
                var result = await _pspApiHttpClient.PostAsync("/Merchant", merchantDTO);

                if (result == null) return BadRequest();

                var isSuccess = await _merchantService.UpdatePspMerchantId(merchant.MerchantId, result.MerchantId);
                if (!isSuccess) return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("PaymentMethods")]
        public async Task<ActionResult<List<PaymentMethodDTO>>> GetPaymentMethods()
        {
            try
            {
                var result = await _pspApiHttpClient.GetAsync<List<PaymentMethodDTO>>("PaymentMethod");
                if (result == null) return BadRequest("Unexpected exception while getting PaymentMethods from PSP");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("PaymentMethods/Import")]
        public async Task<ActionResult<List<PaymentMethodDTO>>> ImportPaymentMethods()
        {
            try
            {
                var result = await _pspApiHttpClient.GetAsync<List<PaymentMethodDTO>>("PaymentMethod");
                if (result == null) return BadRequest("Unexpected exception while getting PaymentMethods from PSP");

                await _paymentMethodService.ImportFromPspAsync(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("PaymentMethods/ByMerchantId/{userId}")]
        public async Task<ActionResult<List<PaymentMethodMerchantODTO>>> Unsubscribe([FromRoute] string userId)
        {
            try
            {
                var merchant = await _merchantService.GetMerchantByUserIdAsync(userId);
                if (merchant == null || merchant.PspMerchantId == null) return NotFound("Merchant not found");

                var result = await _pspApiHttpClient.GetAsync<List<PaymentMethodMerchantODTO>>($"PaymentMethod/ByMerchantId/{merchant.PspMerchantId}");
                if (result == null) return BadRequest("Unexpected exception while getting PaymentMethods from PSP");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PaymentMethods/Subscribe")]
        public async Task<ActionResult> Unsubscribe([FromBody] PaymentMethodSubscribeIDTO paymentMethodSubscribeIDTO)
        {
            try
            {
                var merchant = await _merchantService.GetMerchantByUserIdAsync(paymentMethodSubscribeIDTO.UserId);
                if (merchant == null || merchant.PspMerchantId == null) return NotFound("Merchant not found");

                var paymentMethod = await _paymentMethodService.GetPaymentMethodByIdAsync(paymentMethodSubscribeIDTO.PaymentMethodId);
                if (paymentMethod == null) return NotFound($"Payment Method {paymentMethodSubscribeIDTO.PaymentMethodId} not found");

                var pspPaymentMethodSubscribe = new PspPaymentMethodSubscribeIDTO(paymentMethodSubscribeIDTO.Code.ToString(), paymentMethodSubscribeIDTO.Secret)
                {
                    MerchantId = (int)merchant.PspMerchantId,
                    PaymentMethodId = paymentMethod.PspPaymentMethodId,
                    InstitutionId = paymentMethodSubscribeIDTO.InstitutionId
                };

                var isSuccess = await _pspApiHttpClient.PutAsync($"PaymentMethod/Subscribe", pspPaymentMethodSubscribe);
                if (!isSuccess) return BadRequest("Unexpected exception  from PSP while subscribing to PaymentMethod");

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PaymentMethods/Unsubscribe/{paymentMethodId};{userId}")]
        public async Task<ActionResult> Unsubscribe([FromRoute] int paymentMethodId, [FromRoute] string userId)
        {
            try
            {
                var merchant = await _merchantService.GetMerchantByUserIdAsync(userId);
                if (merchant == null || merchant.PspMerchantId == null) return NotFound("Merchant not found");

                var paymentMethod = await _paymentMethodService.GetPaymentMethodByIdAsync(paymentMethodId);
                if (paymentMethod == null) return NotFound($"Payment Method {paymentMethodId} not found");

                var isSuccess = await _pspApiHttpClient.PutAsync($"PaymentMethod/Unsubscribe/{paymentMethod.PspPaymentMethodId};{merchant.PspMerchantId}");
                if (!isSuccess) return BadRequest("Unexpected exception  from PSP while unsubscribing from PaymentMethod");

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Institutions/{paymentMethodId}")]
        public async Task<ActionResult<List<InstitutionODTO>>> GetInstitutions([FromRoute] int paymentMethodId)
        {
            var institutions = new List<InstitutionODTO>();

            try
            {
                var paymentMethod = await _paymentMethodService.GetPaymentMethodByIdAsync(paymentMethodId);
                if (paymentMethod == null) return NotFound($"Payment Method {paymentMethodId} not found");

                institutions = await _pspApiHttpClient.GetAsync<List<InstitutionODTO>>($"PaymentMethod/Institutions/{paymentMethod.PspPaymentMethodId}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(institutions);
        }
    }
}
