using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebShop.DTO.Enums;

namespace WebShop.WebApi.Models
{
    [Table("Transactions", Schema = "dbo")]
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }
        public int InvoiceId {get; set; }
        public int PaymentMethodId { get; set; }
        public DateTime CreatedTimestamp { get; set; }

        [Column(TypeName = "nvarchar(24)")]
        public TransactionStatus TransactionStatus { get; set; }

        public Invoice? Invoice { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public ICollection<TransactionLog>? TransactionLogs { get; set; }
    }
}
