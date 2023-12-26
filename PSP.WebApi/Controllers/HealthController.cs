using Microsoft.AspNetCore.Mvc;

namespace PSP.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public ActionResult Health()
        {
            return Ok("PSP OK");
        }
    }
}
