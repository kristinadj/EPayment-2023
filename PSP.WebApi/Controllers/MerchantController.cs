using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PSP.WebApi.DTO.Input;
using PSP.WebApi.DTO.Output;
using PSP.WebApi.Services;

namespace PSP.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class MerchantController : ControllerBase
    {
        private readonly IMerchantService _merchantServices;

        public MerchantController(IMerchantService merchantServices)
        {
            _merchantServices = merchantServices;
        }

        [HttpPost]
        public async Task<ActionResult<MerchantODTO>> AddMerchant([FromBody] MerchantIDTO merchant)
        {
            try
            {
                var result = await _merchantServices.AddMerchantAsync(merchant);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
