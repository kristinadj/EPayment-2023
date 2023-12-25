using EntityFrameworkCore.EncryptColumn.Attribute;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankPaymentService.WebApi.Models
{
    [Table("Merchants", Schema = "dbo")]
    public class Merchant
    {
        [Key]
        public int MerchantId { get; set; }
        public int PaymentServiceMerchantId { get; set; }
        public int BankMerchantId { get; set; }
        public int BankId { get; set; }

        [EncryptColumn]
        public string Secret { get; set; }

        [Required]
        [StringLength(50)]
        public string PreferredAccountNumber { get; set; }

        public Bank? Bank { get; set; }
        public ICollection<Invoice>? Invoices { get; set; }

        public Merchant(string preferredAccountNumber, string secret)
        {
            PreferredAccountNumber = preferredAccountNumber;
            Secret = secret;
        }
    }
}
