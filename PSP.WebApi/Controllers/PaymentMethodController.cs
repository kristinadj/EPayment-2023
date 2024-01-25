using Base.DTO.Input;
using Base.DTO.Output;
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
            try
            {
                var result = await _paymentMethodServices.GetPaymentMethodsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<PaymentMethodODTO>> AddPaymentMethod([FromBody] PaymentMethodIDTO paymentMethod)
        {
            try
            {
                var result = await _paymentMethodServices.AddPaymentMethodAsync(paymentMethod);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ByMerchantId/{merchantId}")]
        public async Task<ActionResult<List<PaymentMethodMerchantODTO>>> GetPaymentMethodsByMerchantId([FromRoute] int merchantId)
        {
            try
            {
                var result = await _paymentMethodServices.GetPaymentMethodsByMerchantIdAsync(merchantId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Active/ByMerchantId/{merchantId};{recurringPayment}")]
        public async Task<ActionResult<List<PaymentMethodODTO>>> GetActivePaymentMethodsByMerchantId([FromRoute] int merchantId, [FromRoute] bool recurringPayment)
        {
            try
            {
                var result = await _paymentMethodServices.GetActivePaymentMethodsByMerchantIdAsync(merchantId, recurringPayment);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Subscribe")]
        public async Task<ActionResult<bool>> Subscribe([FromBody] PspPaymentMethodSubscribeIDTO paymentMethodSubscribe)
        {
            try
            {
                var paymentMethod = await _paymentMethodServices.GetPaymentMethodByIdAsync(paymentMethodSubscribe.PaymentMethodId);
                var merchant = await _merchantService.GetMerchantByIdAsync(paymentMethodSubscribe.MerchantId);
                var result = await _paymentMethodServices.SubscribeAsync(paymentMethodSubscribe);

                var updateMerchantCredentials = new UpdateMerchantCredentialsIDTO(paymentMethodSubscribe.Code, paymentMethodSubscribe.Secret)
                {
                    PaymentServiceMerchantId = merchant!.MerchantId,
                    InstitutionId = paymentMethodSubscribe.InstitutionId
                };

                try
                {
                    var isSuccess = await _consulHttpClient.PutAsync(paymentMethod!.ServiceName, $"{paymentMethod.ServiceApiSufix}/PaymentServiceProvider/Merchant/UpdateCredentials", updateMerchantCredentials);
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

        [HttpGet("Institutions/{paymentMethodId}")]
        public async Task<ActionResult<List<InstitutionODTO>>> GetInstitutions([FromRoute] int paymentMethodId)
        {
            var institutions = new List<InstitutionODTO>();

            try
            {
                var paymentMethod = await _paymentMethodServices.GetPaymentMethodByIdAsync(paymentMethodId);
                if (paymentMethod == null) return NotFound();

                institutions = await _consulHttpClient.GetAsync<List<InstitutionODTO>>(paymentMethod.ServiceName, $"{paymentMethod.ServiceApiSufix}/Institutions");
            }
            catch (Exception)
            {
            }

            return Ok(institutions);
        }
    }
}
