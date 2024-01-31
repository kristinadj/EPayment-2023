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
        public int IssuerTransactionId { get; set; }
        public int TransactionId { get; set; }
        public int? AquirerTransactionId { get; set; }
        public DateTime AquirerTimestamp { get; set; }

        public Transaction? Transaction { get; set; }
    }
}
