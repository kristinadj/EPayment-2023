using BankPaymentService.WebApi.AppSettings;
using Base.Services.AppSettings;
using Base.Services.Clients;
using Base.DTO.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Base.DTO.Input;
using BankPaymentService.WebApi.Services;

namespace BankPaymentService.WebApi.Controllers
{
    [Route("api/card/PaymentServiceProvider")]
    [ApiController]
    public class PaymentServiceProviderCardController : ControllerBase
    {
        private readonly CardPaymentMethod _cardPaymentMethod;
        private readonly ConsulAppSettings _consulAppSettings;
        private readonly IConsulHttpClient _consulHttpClient;

        private readonly IMerchantService _merchantService;

        public PaymentServiceProviderCardController(
            IOptions<CardPaymentMethod> cardPaymentMethod,
            IOptions<ConsulAppSettings> consulAppSettings, 
            IConsulHttpClient consulHttpClient,
            IMerchantService merchantService)
        {
            _cardPaymentMethod = cardPaymentMethod.Value;
            _consulAppSettings = consulAppSettings.Value;
            _consulHttpClient = consulHttpClient;
            _merchantService = merchantService; 
        }

        [HttpPost("Register")]
        public async Task<ActionResult<PaymentMethodDTO>> RegisterCard()
        {
            var paymentMethod = new PaymentMethodDTO(_cardPaymentMethod.Name, _consulAppSettings.Service, _cardPaymentMethod.ServiceApiSufix);
            var result = await _consulHttpClient.PostAsync(_cardPaymentMethod.PspServiceName, _cardPaymentMethod.PspRegisterApiEndpoint, paymentMethod);

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
