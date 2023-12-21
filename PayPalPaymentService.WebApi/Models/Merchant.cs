using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PayPalPaymentService.WebApi.Models
{
    [Table("Merchants", Schema = "dbo")]
    public class Merchant
    {
        [Key]
        public int MerchantId { get; set; }
        public int PaymentServiceMerchantId { get; set; }
        public string Email { get; set; }
        public string Secret { get; set; }

        public ICollection<Invoice>? Invoices { get; set; }

        public Merchant(string email, string secret)
        {
            Email = email;
            Secret = secret;
        }
    }
}
