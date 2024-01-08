using Base.DTO.Enums;
using System.ComponentModel.DataAnnotations;

namespace Base.DTO.Input
{
    public class PaymentRequestIDTO
    {
        public int MerchantId { get; set; }
        public double Amount { get; set; }

        [Required]
        [StringLength(3)]
        public string CurrencyCode { get; set; } = string.Empty;
        public int ExternalInvoiceId { get; set; }
        public DateTime Timestamp { get; set; }

        [Required]
        [StringLength(70)]
        public string TransactionSuccessUrl { get; set; } = string.Empty;

        [Required]
        [StringLength(70)]
        public string TransactionFailureUrl { get; set; } = string.Empty;

        [Required]
        [StringLength(70)]
        public string TransactionErrorUrl { get; set; } = string.Empty;

        public InvoiceType InvoiceType { get; set; }
    }
}
