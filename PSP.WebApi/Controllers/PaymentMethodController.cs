using Base.DTO.Input;
using Base.DTO.Shared;
using Base.Services.Clients;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PSP.WebApi.DTO.Input;
using PSP.WebApi.DTO.Output;
using PSP.WebApi.Services;

namespace PSP.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IPaymentMethodService _paymentMethodServices;
        private readonly IMerchantService _merchantService;
        private readonly IConsulHttpClient _consulHttpClient;

        public PaymentMethodController(IPaymentMethodService paymentMethodServices, IMerchantService merchantService, IConsulHttpClient consulHttpClient)
        {
            _paymentMethodServices = paymentMethodServices;
            _merchantService = merchantService;
            _consulHttpClient = consulHttpClient;
        }

        [HttpGet]
        public async Task<ActionResult<List<PaymentMethodODTO>>> GetPaymentMethods()
        {
            var result = await _paymentMethodServices.GetPaymentMethodsAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<PaymentMethodODTO>> AddPaymentMethod([FromBody] PaymentMethodIDTO paymentMethod)
        {
            var result = await _paymentMethodServices.AddPaymentMethodAsync(paymentMethod);
            
            if (result == null) return BadRequest();

            return Ok(result);
        }

        [HttpGet("ByMerchantId/{merchantId}")]
        public async Task<ActionResult<List<PaymentMethodMerchantODTO>>> GetPaymentMethodsByMerchantId([FromRoute] int merchantId)
        {
            var result = await _paymentMethodServices.GetPaymentMethodsByMerchantIdAsync(merchantId);
            return Ok(result);
        }

        [HttpGet("Active/ByMerchantId/{merchantId}")]
        public async Task<ActionResult<List<PaymentMethodODTO>>> GetActivePaymentMethodsByMerchantId([FromRoute] int merchantId)
        {
            var result = await _paymentMethodServices.GetActivePaymentMethodsByMerchantIdAsync(merchantId);
            return Ok(result);
        }

        [HttpPut("Subscribe")]
        public async Task<ActionResult<bool>> Subscribe([FromBody] PspPaymentMethodSubscribeIDTO paymentMethodSubscribe)
        {
            try
            {
                var paymentMethod = await _paymentMethodServices.GetPaymentMethodByIdAsync(paymentMethodSubscribe.PaymentMethodId);
                if (paymentMethod == null) return NotFound();

                var merchant = await _merchantService.GetMerchantByIdAsync(paymentMethodSubscribe.MerchantId);
                if (merchant == null) return NotFound();    

                var result = await _paymentMethodServices.SubscribeAsync(paymentMethodSubscribe);

                var updateMerchantCredentials = new UpdateMerchantCredentialsIDTO(paymentMethodSubscribe.Code, paymentMethodSubscribe.Secret)
                {
                    PaymentServiceMerchantId = merchant.MerchantId
                };

                try
                {
                    var isSuccess = await _consulHttpClient.PutAsync(paymentMethod.ServiceName, "Merchant/UpdateCredentials", updateMerchantCredentials);
                    if (!isSuccess) return BadRequest();
                }
                catch (HttpRequestException)
                {
                    return BadRequest();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Unsubscribe/{paymentMethodId};{merchantId}")]
        public async Task<ActionResult<bool>> Unsubscribe([FromRoute] int paymentMethodId, [FromRoute] int merchantId)
        {
            try
            {
                var result = await _paymentMethodServices.UnsubscribeAsync(paymentMethodId, merchantId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
