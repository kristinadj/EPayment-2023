using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PayPalPaymentService.WebApi.Enums;

namespace PayPalPaymentService.WebApi.Models
{
    [Table("InvoiceLogs", Schema = "dbo")]
    public class InvoiceLog
    {
        [Key]
        public int InvoiceLogId { get; set; }
        public int InvoiceId { get; set; }

        [Column(TypeName = "nvarchar(24)")]
        public TransactionStatus TransactionStatus { get; set; }
        public DateTime Timestamp { get; set; }

        public Invoice? Invoice { get; set; }
    }
}
