using System.ComponentModel.DataAnnotations;

namespace WebShop.DTO.Input
{
    public class AuthenticateIDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public AuthenticateIDTO()
        {
            Email = string.Empty;
            Password = string.Empty;
        }

        public AuthenticateIDTO(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
