using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank1.WebApi.Models
{
    [Table("AqurierTransactions", Schema = "dbo")]
    public class AcqurierTransaction
    {
        [Key]
        public int AcqurierTransactionId { get; set; }
        public int TransactionId { get; set; }
        public int? IssuerTransactionId { get; set; }
        public DateTime IssuerTimestamp { get; set; }

        public Transaction? Transaction { get; set; }
    }
}
