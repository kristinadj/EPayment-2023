using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Bank2.WebApi.Enums;

namespace Bank2.WebApi.Models
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
        public int? SenderAccountId { get; set; }
        public int? IssuerTransactionId { get; set; }
        public DateTime? IssuerTimestamp { get; set; }
        public int ReceiverAccountId { get; set; }

        [Column(TypeName = "nvarchar(24)")]
        public TransactionStatus TransactionStatus { get; set; }
        public DateTime Timestamp { get; set; }

        [Required]
        [StringLength(70)]
        public string TransactionSuccessUrl { get; set; }

        [Required]
        [StringLength(70)]
        public string TransactionFailureUrl { get; set; }

        [Required]
        [StringLength(70)]
        public string TransactionErrorUrl { get; set; }

        public Currency? Currency { get; set; }
        public Account? SenderAccount { get; set; }
        public Account? ReceiverAccount { get; set; }
        public ICollection<TransactionLog>? TransactionLogs { get; set; }

        public Transaction(string description, string transactionSuccessUrl, string transactionFailureUrl, string transactionErrorUrl)
        {
            Description = description;
            TransactionSuccessUrl = transactionSuccessUrl;
            TransactionFailureUrl = transactionFailureUrl;
            TransactionErrorUrl = transactionErrorUrl;
        }
    }
}
