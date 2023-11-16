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

        public PaymentMethodController(IPaymentMethodService paymentMethodServices)
        {
            _paymentMethodServices = paymentMethodServices;
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
    }
}
