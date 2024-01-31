using Bank.DTO.Input;
using Bank.DTO.Output;
using Bank2.WebApi.Models;
using Bank2.WebApi.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bank2.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<Customer> _userManager;
        private readonly ITokenCreationService _tokenCreationService;
        public UsersController(
            UserManager<Customer> userManager,
            ITokenCreationService tokenCreationService)
        {
            _userManager = userManager;
            _tokenCreationService = tokenCreationService;
        }

        [HttpPost("Authenticate")]
        public async Task<ActionResult<AuthenticationODTO>> Login([FromBody] AuthenticateIDTO authenticateDTO)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(authenticateDTO.Email);
                if (user == null) return BadRequest("Bad credentials");

                var isPasswordValid = await _userManager.CheckPasswordAsync(user, authenticateDTO.Password);
                if (!isPasswordValid) return BadRequest("Bad credentials");

                var token = _tokenCreationService.CreateToken(user);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
