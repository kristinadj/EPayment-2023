using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebShop.DTO.Input;
using WebShop.DTO.Output;
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
            try
            {
                var result = await _shoppingCartService.AddItemInShoppingCartAsync(itemDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{shoppingCartItemId}")]
        public async Task<ActionResult> AddShoppingCartItem([FromRoute] int shoppingCartItemId)
        {
            try
            {
                var isSuccess = await _shoppingCartService.DeleteItemInShoppingCartAsync(shoppingCartItemId);
                if (!isSuccess) return NotFound($"ShoppingCartItem {shoppingCartItemId} not found");

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
