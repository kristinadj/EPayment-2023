using Base.Services.AppSettings;
using Base.Services.Clients;
using Base.DTO.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace PayPalPaymentService.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentServiceProviderController : ControllerBase
    {
        private readonly PaymentMethod _paymentMethod;
        private readonly ConsulAppSettings _consulAppSettings;
        private readonly IConsulHttpClient _consulHttpClient;

        public PaymentServiceProviderController(IOptions<PaymentMethod> paymentMethod, IOptions<ConsulAppSettings> consulAppSettings, IConsulHttpClient consulHttpClient)
        {
            _paymentMethod = paymentMethod.Value;
            _consulAppSettings = consulAppSettings.Value;
            _consulHttpClient = consulHttpClient;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<PaymentMethodDTO>> Register()
        {
            var paymentMethod = new PaymentMethodDTO(_paymentMethod.Name, _consulAppSettings.Service, _paymentMethod.ServiceApiSufix);
            var result = await _consulHttpClient.PostAsync(_paymentMethod.PspServiceName, _paymentMethod.PspRegisterApiEndpoint, paymentMethod);

            if (result == null) return BadRequest();

            return Ok(result);
        }
    }
}
