using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Bank2.WebApi.Enums;

namespace Bank2.WebApi.Models
{
    [Table("IssuerTransactions", Schema = "dbo")]
    public class IssuerTransaction
    {
        [Key]
        public int TransactionId { get; set; }
        public double Amount { get; set; }
        public int CurrencyId { get; set; }

        [Required]
        public string Description { get; set; }
        public int IssuerAccountId { get; set; }
        public int AquirerTransactionId { get; set; }
        public DateTime AquirerTimestamp { get; set; }

        [Column(TypeName = "nvarchar(24)")]
        public TransactionStatus TransactionStatus { get; set; }
        public DateTime Timestamp { get; set; }

        public Currency? Currency { get; set; }
        public Account? IsuerAccount { get; set; }

        public IssuerTransaction(string description)
        {
            Description = description;
        }
    }
}
