using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebShop.DTO.Input;
using WebShop.DTO.Output;
using WebShop.WebApi.Enums;
using WebShop.WebApi.Models;
using WebShop.WebApi.Services;

namespace WebShop.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenCreationService _tokenCreationService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IMapper _mapper;
        public UsersController(
            UserManager<User> userManager,
            ITokenCreationService tokenCreationService,
            IShoppingCartService shoppingCartService,
            IMapper mapper)
        {
            _userManager = userManager;
            _tokenCreationService = tokenCreationService;
            _shoppingCartService = shoppingCartService;
            _mapper = mapper;
        }


        [HttpPost("Register")]
        public async Task<ActionResult> RegisterAsync([FromBody] UserIDTO userDTO)
        {
            if (!userDTO.Password.Equals(userDTO.ConfirmPassword))
                return BadRequest("Passwords doesn't match");

            var user = await _userManager.FindByEmailAsync(userDTO.Email);
            if (user != null)
                return BadRequest("Email is taken");

            user = _mapper.Map<User>(userDTO);
            user.Role = Role.BUYER;

            var result = await _userManager.CreateAsync(user, userDTO.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _shoppingCartService.CreateShoppingCartAsync(user.Id);
            return Ok();
        }

        [HttpPost("Authenticate")]
        public async Task<ActionResult<AuthenticationODTO>> Login([FromBody] AuthenticateIDTO authenticateDTO)
        {
            var user = await _userManager.FindByEmailAsync(authenticateDTO.Email);
            if (user == null)
                return BadRequest("Bad credentials");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, authenticateDTO.Password);
            if (!isPasswordValid)
                return BadRequest("Bad credentials");

            var token = _tokenCreationService.CreateToken(user);
            return Ok(token);
        }
    }
}
