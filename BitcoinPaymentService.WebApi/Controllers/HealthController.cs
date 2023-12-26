using Microsoft.AspNetCore.Mvc;

namespace BitcoinPaymentService.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public ActionResult Health()
        {
            return Ok("Bitcoin Payment Service OK");
        }
    }
}
