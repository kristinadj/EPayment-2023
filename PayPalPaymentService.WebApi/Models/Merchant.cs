using EntityFrameworkCore.EncryptColumn.Attribute;
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
        public string ClientId { get; set; }

        [EncryptColumn]
        public string Secret { get; set; }

        public string? PayPalBillingPlanProductId { get; set; }

        public string? PayPalBillingPlanId { get; set; }

        public ICollection<Invoice>? Invoices { get; set; }

        public Merchant(string clientId, string secret)
        {
            ClientId = clientId;
            Secret = secret;
        }
    }
}
