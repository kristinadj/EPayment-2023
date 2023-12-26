using Microsoft.AspNetCore.Mvc;

namespace PayPalPaymentService.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public ActionResult Health()
        {
            return Ok("PayPal Payment Service OK");
        }
    }
}
