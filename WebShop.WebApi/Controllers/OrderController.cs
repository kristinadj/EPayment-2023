using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebShop.DTO.Output;
using WebShop.WebApi.Services;

namespace WebShop.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("ById/{orderId}")]
        public async Task<ActionResult<OrderODTO>> GetById([FromRoute] int orderId)
        {
            try
            {
                var result = await _orderService.GetOrderByIdAsync(orderId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ByInvoiceId/{invoiceId}")]
        public async Task<ActionResult<OrderODTO>> GetByInvoiceId([FromRoute] int invoiceId)
        {
            try
            {
                var result = await _orderService.GetOrderByInvoiceIdAsync(invoiceId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Cancel/{orderId}")]
        public async Task<ActionResult> CancelOrder([FromRoute] int orderId)
        {
            try
            {
                var result = await _orderService.CancelOrderAsync(orderId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
