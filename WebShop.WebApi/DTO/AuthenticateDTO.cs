using System.ComponentModel.DataAnnotations;

namespace WebShop.WebApi.DTO
{
    public class AuthenticateDTO
    {
        [Required]
        public string Email { get; set; }

        [Required] 
        public string Password { get; set; }

        public AuthenticateDTO(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
