using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebShop.DTO.Enums;
using WebShop.DTO.Input;
using WebShop.DTO.Output;
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
        private readonly ISubscriptionPlanService _subscriptionPlanService;
        private readonly IMapper _mapper;
        public UsersController(
            UserManager<User> userManager,
            ITokenCreationService tokenCreationService,
            IShoppingCartService shoppingCartService,
            ISubscriptionPlanService subscriptionPlanService,
            IMapper mapper)
        {
            _userManager = userManager;
            _tokenCreationService = tokenCreationService;
            _shoppingCartService = shoppingCartService;
            _subscriptionPlanService = subscriptionPlanService;
            _mapper = mapper;
        }


        [HttpPost("Register")]
        public async Task<ActionResult<AuthenticationODTO>> RegisterAsync([FromBody] UserIDTO userDTO)
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
            var token = _tokenCreationService.CreateToken(user, false);
            return Ok(token);
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

            var isSubscriptionValid = await _subscriptionPlanService.IsSubscriptionPlanValidAsync(user.Id);
            var token = _tokenCreationService.CreateToken(user, isSubscriptionValid);
            return Ok(token);
        }
    }
}
