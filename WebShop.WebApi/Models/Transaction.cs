using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;

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
        public TransactionStatus TransactionStatus { get; set; }

        public Invoice? Invoice { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
    }
}
