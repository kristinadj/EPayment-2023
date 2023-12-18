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
        private readonly PspAppSettings _pspAppSettings;
        private readonly WebShopAppSettings _webShopAppSettings;
        private readonly ConsulAppSettings _consulAppSettings;

        private readonly IMerchantService _merchantService;
        private readonly IPaymentMethodService _paymentMethodService;
        private readonly IConsulHttpClient _consulHttpClient;

        public PaymentServiceProviderController(
            IMerchantService merchantService,
            IPaymentMethodService paymentMethodService,
            IOptions<PspAppSettings> pspAppSettings,
            IOptions<ConsulAppSettings> consulAppSettings,
            IOptions<WebShopAppSettings> webShopAppSettings,
            IConsulHttpClient consulHttpClient)
        {
            _merchantService = merchantService;
            _paymentMethodService = paymentMethodService;
            _pspAppSettings = pspAppSettings.Value;
            _consulAppSettings = consulAppSettings.Value;
            _webShopAppSettings = webShopAppSettings.Value;
            _consulHttpClient = consulHttpClient;
        }

        [HttpGet("Merchant/IsRegistered/{userId}")]
        public async Task<ActionResult<MerchantDTO>> IsMerchantRegistered([FromRoute] string userId)
        {
            var merchant = await _merchantService.GetMerchantByUserIdAsync(userId);
            if (merchant == null || merchant.PspMerchantId == null) return NotFound();

            return Ok();
        }

        [HttpPost("Merchant/Register/{userId}")]
        public async Task<ActionResult<MerchantDTO>> Register([FromRoute] string userId)
        {
            var merchant = await _merchantService.GetMerchantByUserIdAsync(userId);
            if (merchant == null) return NotFound();

            var merchantDTO = new MerchantDTO(merchant.MerchantId.ToString(), merchant.User!.Name, merchant.User!.Address!, merchant.User!.PhoneNumber!, merchant.User!.Email!, _consulAppSettings.Service)
            {
                TransactionSuccessUrl = $"{_webShopAppSettings.ClientUrl}/invoice/@INVOICE_ID@/success",
                TransactionFailureUrl = $"{_webShopAppSettings.ClientUrl}/invoice/@INVOICE_ID@/failure",
                TransactionErrorUrl = $"{_webShopAppSettings.ClientUrl}/invoice/@INVOICE_ID@/error",
            };
            var result = await _consulHttpClient.PostAsync(_pspAppSettings.ServiceName, _pspAppSettings.RegisterMerchantApiEndpoint, merchantDTO);

            if (result == null) return BadRequest();

            var isSuccess = await _merchantService.UpdatePspMerchantId(merchant.MerchantId, result.MerchantId);
            if (!isSuccess) return NotFound();

            return Ok(result);
        }

        [HttpGet("PaymentMethods")]
        public async Task<ActionResult<List<PaymentMethodDTO>>> GetPaymentMethods()
        {
            var result = await _consulHttpClient.GetAsync<List<PaymentMethodDTO>>(_pspAppSettings.ServiceName, "/api/PaymentMethod");
            if (result == null) return BadRequest();

            return Ok(result);
        }

        [HttpPost("PaymentMethods/Import")]
        public async Task<ActionResult<List<PaymentMethodDTO>>> ImportPaymentMethods()
        {
            var result = await _consulHttpClient.GetAsync<List<PaymentMethodDTO>>(_pspAppSettings.ServiceName, "/api/PaymentMethod");
            if (result == null) return BadRequest();

            await _paymentMethodService.ImportFromPspAsync(result);

            return Ok(result);
        }

        [HttpGet("PaymentMethods/ByMerchantId/{userId}")]
        public async Task<ActionResult<List<PaymentMethodMerchantODTO>>> Unsubscribe([FromRoute] string userId)
        {
            try
            {
                var merchant = await _merchantService.GetMerchantByUserIdAsync(userId);
                if (merchant == null || merchant.PspMerchantId == null) return NotFound();

                var result = await _consulHttpClient.GetAsync<List<PaymentMethodMerchantODTO>>(_pspAppSettings.ServiceName, $"/api/PaymentMethod/ByMerchantId/{merchant.PspMerchantId}");
                if (result == null) return BadRequest();

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
                if (merchant == null || merchant.PspMerchantId == null) return NotFound();

                var paymentMethod = await _paymentMethodService.GetPaymentMethodById(paymentMethodSubscribeIDTO.PaymentMethodId);
                if (paymentMethod == null) return NotFound();

                var pspPaymentMethodSubscribe = new PspPaymentMethodSubscribeIDTO(paymentMethodSubscribeIDTO.Secret)
                {
                    MerchantId = (int)merchant.PspMerchantId,
                    PaymentMethodId = paymentMethod.PspPaymentMethodId,
                    Code = paymentMethodSubscribeIDTO.Code
                };

                var isSuccess = await _consulHttpClient.PutAsync(_pspAppSettings.ServiceName, $"/api/PaymentMethod/Subscribe", pspPaymentMethodSubscribe);
                if (!isSuccess) return BadRequest();

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
                if (merchant == null || merchant.PspMerchantId == null) return NotFound();

                var paymentMethod = await _paymentMethodService.GetPaymentMethodById(paymentMethodId);
                if (paymentMethod == null) return NotFound();

                var isSuccess = await _consulHttpClient.PutAsync(_pspAppSettings.ServiceName, $"/api/PaymentMethod/Unsubscribe/{paymentMethod.PspPaymentMethodId};{merchant.PspMerchantId}");
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
