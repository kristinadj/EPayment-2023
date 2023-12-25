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
        public string Token { get; set; }

        [EncryptColumn]
        public string PairingCode { get; set; }

        public ICollection<Invoice>? Invoices { get; set; }

        public Merchant(string token, string pairingCode)
        {
            Token = token;
            PairingCode = pairingCode;
        }
    }
}
