using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebShop.WebApi.DTO;
using WebShop.WebApi.Services;

namespace WebShop.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class SubscriptionPlansController : ControllerBase
    {
        private readonly ISubscriptionPlanService _subscriptionPlanService;

        public SubscriptionPlansController(ISubscriptionPlanService subscriptionPlanService)
        {
            _subscriptionPlanService = subscriptionPlanService;
        }

        [HttpGet]
        public async Task<ActionResult<List<SubscriptionPlanDTO>>> GetSubscriptionPlans()
        {
            var subscriptionPlans = await _subscriptionPlanService.GetSubscriptionPlansAsync();
            return Ok(subscriptionPlans);
        }
    }
}
