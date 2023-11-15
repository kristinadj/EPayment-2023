using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.WebApi.Models
{
    [Table("UserSubscriptionPlans", Schema = "dbo")]
    public class UserSubscriptionPlan
    {
        [Key]
        public int UserSubscriptionPlanId { get; set; }
        public string UserId { get; set; }
        public int SubscriptionPlanId { get; set; }
        public DateTime StartTimestamp { get; set; }
        public DateTime EndTimestamp { get; set; }
        public int InvoiceId { get; set; }

        public User? User { get; set; }
        public SubscriptionPlan? SubscriptionPlan { get; set; }
        public Invoice? Invoice { get; set; }

        public UserSubscriptionPlan(string userId)
        {
            UserId = userId;
        }
    }
}
