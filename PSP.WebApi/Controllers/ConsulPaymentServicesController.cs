using Microsoft.AspNetCore.Mvc;
using PSP.WebApi.Services;

namespace PSP.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsulPaymentServicesController : ControllerBase
    {
        private readonly IConsulService _consulServices;
        public ConsulPaymentServicesController(IConsulService consulServices)
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
