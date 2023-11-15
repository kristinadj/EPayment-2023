using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebShop.DTO.Input;
using WebShop.WebApi.Services;

namespace WebShop.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    [Authorize]
    public class ShoppingCartItemController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartItemController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpPost]
        public async Task<ActionResult<bool>> AddShoppingCartItem([FromBody] ShoppingCartItemIDTO itemDTO)
        {
            var result = await _shoppingCartService.AddItemInShoppingCartAsync(itemDTO);
            return Ok(result);
        }
    }
}
