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
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpGet("ByUser/{userId}")]
        public async Task<ActionResult<ShoppingCartODTO>> GetShoppingCart([FromRoute] string userId)
        {
            var result = await _shoppingCartService.GetShoppingCartByUserAsync(userId);
            return Ok(result);
        }
    }
}
