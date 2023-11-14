using System.ComponentModel.DataAnnotations;

namespace WebShop.DTO
{
    public class AuthenticateDTO
    {
        [Required]
        public string Email { get; set; }

        [Required] 
        public string Password { get; set; }

        public AuthenticateDTO() 
        { 
            Email = string.Empty;
            Password = string.Empty;
        }

        public AuthenticateDTO(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
