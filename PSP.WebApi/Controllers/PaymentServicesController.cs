using Microsoft.AspNetCore.Mvc;
using PSP.WebApi.Services;

namespace PSP.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentServicesController : ControllerBase
    {
        private readonly IConsulServices _consulServices;
        public PaymentServicesController(IConsulServices consulServices)
        {
            _consulServices = consulServices;
        }

        [HttpGet]
        public async Task<ActionResult<Dictionary<string, Uri>>> GetPaymentServices()
        {
            var result = await _consulServices.GetPaymentServicesAsync();
            return Ok(result);
        }
    }
}
