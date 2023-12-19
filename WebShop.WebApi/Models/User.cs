using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebShop.DTO.Enums;

namespace WebShop.WebApi.Models
{
    [Table("Users", Schema = "dbo")]
    public class User : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(100)]
        public string? Address { get; set; }

        public Role Role { get; set; }

        public ICollection<UserSubscriptionPlan>? UserSubscriptionPlans { get; set; }
        public ICollection<Invoice>? Invoices { get; set; }

        public User(string name)
        {
            Name = name;
        }
    }
}
