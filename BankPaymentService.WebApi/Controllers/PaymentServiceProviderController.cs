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
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentServiceProviderController : ControllerBase
    {
        private readonly CardPaymentMethod _cardPaymentMethod;
        private readonly QrCodePaymentMethod _qrCodePaymentMethod;
        private readonly ConsulAppSettings _consulAppSettings;
        private readonly IConsulHttpClient _consulHttpClient;

        private readonly IMerchantService _merchantService;

        public PaymentServiceProviderController(
            IOptions<CardPaymentMethod> cardPaymentMethod,
            IOptions<QrCodePaymentMethod> qrCodePaymentMethod,
            IOptions<ConsulAppSettings> consulAppSettings, 
            IConsulHttpClient consulHttpClient,
            IMerchantService merchantService)
        {
            _cardPaymentMethod = cardPaymentMethod.Value;
            _qrCodePaymentMethod = qrCodePaymentMethod.Value;
            _consulAppSettings = consulAppSettings.Value;
            _consulHttpClient = consulHttpClient;
            _merchantService = merchantService; 
        }

        [HttpPost("Register/Card")]
        public async Task<ActionResult<PaymentMethodDTO>> RegisterCard()
        {
            var paymentMethod = new PaymentMethodDTO(_cardPaymentMethod.Name, _consulAppSettings.Service, _cardPaymentMethod.ServiceApiSufix);
            var result = await _consulHttpClient.PostAsync(_cardPaymentMethod.PspServiceName, _cardPaymentMethod.PspRegisterApiEndpoint, paymentMethod);

            if (result == null) return BadRequest();

            return Ok(result);
        }

        [HttpPost("Register/QrCode")]
        public async Task<ActionResult<PaymentMethodDTO>> RegisterQrCode()
        {
            var paymentMethod = new PaymentMethodDTO(_qrCodePaymentMethod.Name, _consulAppSettings.Service, _qrCodePaymentMethod.ServiceApiSufix);
            var result = await _consulHttpClient.PostAsync(_qrCodePaymentMethod.PspServiceName, _qrCodePaymentMethod.PspRegisterApiEndpoint, paymentMethod);

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
