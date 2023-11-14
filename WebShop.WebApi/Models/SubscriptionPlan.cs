using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.WebApi.Models
{
    [Table("SubscriptionPlans", Schema = "dbo")]
    public class SubscriptionPlan
    {
        [Key]
        public int SubscriptionPlanId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Description { get; set; }
        public double Price { get; set; }
        public int CurrencyId { get; set; }
        public int DurationInDays { get; set; }
        public bool AutomaticRenewel { get; set; }

        public Currency? Currency { get; set; }
        public ICollection<UserSubscriptionPlan>? UserSubscriptionPlans { get; set; }

        public SubscriptionPlan(string name, string description) 
        { 
            Name = name;
            Description = description;
        }

    }
}
