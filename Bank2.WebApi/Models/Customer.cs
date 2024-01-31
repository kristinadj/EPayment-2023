using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Bank2.WebApi.Models
{
    [Table("Customers", Schema = "dbo")]
    public class Customer : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(70)]
        public string Address { get; set; } = string.Empty;

        public ICollection<Account>? Accounts { get; set; }
    }
}
