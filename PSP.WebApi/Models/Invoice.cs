using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PSP.WebApi.Enums;
using Base.DTO.Enums;

namespace PSP.WebApi.Models
{
    [Table("Invoices", Schema = "dbo")]
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }
        public int ExternalInvoiceId { get; set; }
        public int MerchantId { get; set; }
        public string IssuedToUserId { get; set; }
        public double TotalPrice { get; set; }
        public int CurrencyId { get; set; }
        public int TransactionId { get; set; }

        [Column(TypeName = "nvarchar(24)")]
        public InvoiceType InvoiceType { get; set; }
        public bool RecurringPayment { get; set; }

        public Merchant? Merchant { get; set; }
        public Currency? Currency { get; set; }
        public Transaction? Transaction { get; set; }

        public Invoice(string issuedToUserId)
        {
            IssuedToUserId = issuedToUserId;
        }
    }
}
