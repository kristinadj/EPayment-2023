﻿using Microsoft.AspNetCore.Authorization;
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
        private readonly IOrderService _orderService;

        public ShoppingCartController(IShoppingCartService shoppingCartService, IOrderService orderService)
        {
            _shoppingCartService = shoppingCartService;
            _orderService = orderService;
        }

        [HttpGet("ByUser/{userId}")]
        public async Task<ActionResult<ShoppingCartODTO>> GetShoppingCart([FromRoute] string userId)
        {
            try
            {
                var result = await _shoppingCartService.GetShoppingCartByUserAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Checkout/{shoppingCartId}")]
        public async Task<ActionResult<OrderODTO?>> Checkout([FromRoute] int shoppingCartId)
        {
            try
            {
                var result = await _orderService.CreateOrderAsync(shoppingCartId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }       
        }
    }
}
