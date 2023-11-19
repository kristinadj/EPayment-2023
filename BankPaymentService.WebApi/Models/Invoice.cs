using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;

namespace BankPaymentService.WebApi.Models
{
    [Table("Invoices", Schema = "dbo")]
    public class Invoice
    {
        [Key]
        public int InvocieId { get; set; }
        public int ExternalInvoiceId { get; set; }
        public int MerchantId { get; set; }
        public double Amount { get; set; }
        public int CurrencyId { get; set; }
        public DateTime Timestamp { get; set;  }

        [Column(TypeName = "nvarchar(24)")]
        public TransactionStatus TransactionStatus { get; set; }

        public Merchant? Merchant { get; set; }
        public Currency? Currency { get; set; }
        public ICollection<InvoiceLog>? InvoiceLogs { get; set;}
    }
}
