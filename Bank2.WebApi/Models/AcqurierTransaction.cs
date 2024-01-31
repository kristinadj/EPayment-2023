using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Bank2.WebApi.Models
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
