using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.DTO.Input
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
