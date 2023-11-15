using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.WebApi.Models
{
    [Table("PaymentMethodMerchants", Schema = "dbo")]
    public class PaymentMethodMerchant
    {
        [Key]
        public int PaymentMethodMerchantId { get; set; }
        public int PaymentMethodId { get; set; }
        public int MerchantId { get; set; }
        public bool IsActive { get; set; }

        public PaymentMethod? PaymentMethod { get; set; }
        public Merchant? Merchant { get; set; }
    }
}
