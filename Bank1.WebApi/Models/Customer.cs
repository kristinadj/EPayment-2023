using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank1.WebApi.Models
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
