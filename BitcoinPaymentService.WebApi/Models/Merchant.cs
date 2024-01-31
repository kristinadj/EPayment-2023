using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EntityFrameworkCore.EncryptColumn.Attribute;

namespace BitcoinPaymentService.WebApi.Models
{
    [Table("Merchants", Schema = "dbo")]
    public class Merchant
    {
        [Key]
        public int MerchantId { get; set; }
        public int PaymentServiceMerchantId { get; set; }

        [EncryptColumn]
        public string? ApiKey { get; set; }

        [EncryptColumn]
        public string? Code { get; set; }

        public ICollection<Invoice>? Invoices { get; set; }
    }
}
