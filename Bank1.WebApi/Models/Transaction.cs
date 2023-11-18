using Bank1.WebApi.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank1.WebApi.Models
{
    [Table("Transactions", Schema = "dbo")]
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }
        public double Amount { get; set; }
        public int CurrencyId { get; set; }

        [Required]
        public string Description { get; set; }
        public int SenderAccountId { get; set; }
        public int ReceiverAccountId { get; set; }

        [Column(TypeName = "nvarchar(24)")]
        public TransactionStatus TransactionStatus { get; set; }
        public DateTime Timestamp { get; set; }

        public Currency? Currency { get; set; }
        public Account? SenderAccount { get; set; }
        public Account? ReceiverAccount { get; set; }
        public ICollection<TransactionLog>? TransactionLogs { get; set; }

        public Transaction(string description)
        {
            Description = description;
        }
    }
}
