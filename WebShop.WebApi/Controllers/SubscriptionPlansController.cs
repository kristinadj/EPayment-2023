using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebShop.DTO.Output;
using WebShop.WebApi.Services;

namespace WebShop.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    [Authorize]
    public class SubscriptionPlansController : ControllerBase
    {
        private readonly ISubscriptionPlanService _subscriptionPlanService;

        public SubscriptionPlansController(ISubscriptionPlanService subscriptionPlanService)
        {
            _subscriptionPlanService = subscriptionPlanService;
        }

        [HttpGet]
        public async Task<ActionResult<List<SubscriptionPlanODTO>>> GetSubscriptionPlans()
        {
            var result = await _subscriptionPlanService.GetSubscriptionPlansAsync();
            return Ok(result);
        }

        [HttpGet("IsValid/{userId}")]
        public async Task<ActionResult<bool>> IsSubscriptionPlanValid([FromRoute] string userId)
        {
            var result = await _subscriptionPlanService.IsSubscriptionPlanValidAsync(userId);
            return Ok(result);
        }
    }
}
