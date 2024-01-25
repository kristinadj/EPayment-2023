using BankPaymentService.WebApi.Services;
using Base.DTO.Output;
using Microsoft.AspNetCore.Mvc;

namespace BankPaymentService.WebApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private readonly IBankService _bankService;

        public BankController(IBankService bankService)
        {
            _bankService = bankService;
        }

        [HttpGet("card/Institutions")]
        public async Task<ActionResult<List<InstitutionODTO>>> GetBanksCard()
        {
            var institutions = await _bankService.GetBanksAsync();
            return Ok(institutions);
        }

        [HttpGet("qrcode/Institutions")]
        public async Task<ActionResult<List<InstitutionODTO>>> GetBanksQrCode()
        {
            var institutions = await _bankService.GetBanksAsync();
            return Ok(institutions);
        }
    }
}
