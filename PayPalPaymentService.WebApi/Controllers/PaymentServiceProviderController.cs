using Base.Services.AppSettings;
using Base.Services.Clients;
using Base.DTO.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Base.DTO.Input;
using PayPalPaymentService.WebApi.Services;

namespace PayPalPaymentService.WebApi.Controllers
{
    [Route("api/paypal/[controller]")]
    [ApiController]
    public class PaymentServiceProviderController : ControllerBase
    {
        private readonly PaymentMethod _paymentMethod;
        private readonly ConsulAppSettings _consulAppSettings;
        private readonly IConsulHttpClient _consulHttpClient;

        private readonly IMerchantService _merchantService;

        public PaymentServiceProviderController(
            IOptions<PaymentMethod> paymentMethod, 
            IOptions<ConsulAppSettings> consulAppSettings, 
            IConsulHttpClient consulHttpClient,
            IMerchantService merchantService)
        {
            _paymentMethod = paymentMethod.Value;
            _consulAppSettings = consulAppSettings.Value;
            _consulHttpClient = consulHttpClient;
            _merchantService = merchantService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<PaymentMethodDTO>> Register()
        {
            var paymentMethod = new PaymentMethodDTO(_paymentMethod.Name, _consulAppSettings.Service, _paymentMethod.ServiceApiSufix)
            {
                SupportsAutomaticPayments = _paymentMethod.SupportsAutomaticPayments
            };
            var result = await _consulHttpClient.PostAsync(_paymentMethod.PspServiceName, _paymentMethod.PspRegisterApiEndpoint, paymentMethod);

            if (result == null) return BadRequest();

            return Ok(result);
        }

        [HttpPut("Merchant/UpdateCredentials")]
        public async Task<ActionResult<PaymentMethodDTO>> UpdateMerchantCredentials(UpdateMerchantCredentialsIDTO updateMerchantCredentialsIDTO)
        {
            var isSuccess = await _merchantService.UpdateMerchantCredentialsAsync(updateMerchantCredentialsIDTO);
            if (!isSuccess) return BadRequest();

            return Ok();
        }
    }
}
