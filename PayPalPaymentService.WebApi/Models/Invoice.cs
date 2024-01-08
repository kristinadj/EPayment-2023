using Base.DTO.Enums;
using PayPalPaymentService.WebApi.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PayPalPaymentService.WebApi.Models
{
    [Table("Invoices", Schema = "dbo")]
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }
        public int ExternalInvoiceId { get; set; }
        public string? PayPalOrderId { get; set; }
        public string? PayPalSubscriptionId { get; set; }
        public string? PayerId { get; set; }
        public int MerchantId { get; set; }
        public double Amount { get; set; }
        public int CurrencyId { get; set; }

        [Column(TypeName = "nvarchar(24)")]

        public InvoiceType InvoiceType { get; set; }

        [Required]
        [StringLength(70)]
        public string TransactionSuccessUrl { get; set; }

        [Required]
        [StringLength(70)]
        public string TransactionFailureUrl { get; set; }

        [Required]
        [StringLength(70)]
        public string TransactionErrorUrl { get; set; }

        public DateTime Timestamp { get; set; }

        [Column(TypeName = "nvarchar(24)")]
        public TransactionStatus TransactionStatus { get; set; }

        public bool RecurringPayment { get; set; }

        public Merchant? Merchant { get; set; }
        public Currency? Currency { get; set; }
        public ICollection<InvoiceLog>? InvoiceLogs { get; set; }

        public Invoice(string transactionSuccessUrl, string transactionFailureUrl, string transactionErrorUrl)
        {
            TransactionSuccessUrl = transactionSuccessUrl;
            TransactionFailureUrl = transactionFailureUrl;
            TransactionErrorUrl = transactionErrorUrl;
        }
    }
}
