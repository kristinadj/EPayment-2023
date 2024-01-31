using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank1.WebApi.Models
{
    [Table("IssuerTransactions", Schema = "dbo")]
    public class IssuerTransaction
    {
        [Key]
        public int IssuerTransactionId { get; set; }
        public int TransactionId { get; set; }
        public int? AquirerTransactionId { get; set; }
        public DateTime AquirerTimestamp { get; set; }

        public Transaction? Transaction { get; set; }
    }
}
