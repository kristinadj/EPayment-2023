using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebShop.DTO.Output;
using WebShop.WebApi.Models;

namespace WebShop.WebApi.Services
{
    public interface ITokenCreationService
    {
        AuthenticationODTO CreateToken(User user);
    }

    public class JwtService : ITokenCreationService
    {

        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AuthenticationODTO CreateToken(User user)
        {
            var expirationMinutes = int.Parse(_configuration["Jwt:ExpiresInMinutes"]);
            var expiration = DateTime.UtcNow.AddMinutes(expirationMinutes);

            var token = CreateJwtToken(
                CreateClaims(user),
                CreateSigningCredentials(),
                expiration
            );

            var tokenHandler = new JwtSecurityTokenHandler();

            return new AuthenticationODTO(tokenHandler.WriteToken(token))
            {
                Expiration = expiration
            };
        }

        private JwtSecurityToken CreateJwtToken(Claim[] claims, SigningCredentials credentials, DateTime expiration) =>
            new(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: expiration,
                signingCredentials: credentials
            );

        private Claim[] CreateClaims(User user) =>
            new[] {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString() )
            };

        private SigningCredentials CreateSigningCredentials() =>
            new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
                ),
                SecurityAlgorithms.HmacSha256
            );
    }
}
