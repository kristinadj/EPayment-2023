using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebShop.DTO.Enums;

namespace WebShop.WebApi.Models
{
    [Table("TransactionLogs", Schema = "dbo")]
    public class TransactionLog
    {
        [Key]
        public int TransactionLogId { get; set; }
        public int TransactionId { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public DateTime Timestamp { get; set; }

        public Transaction? Transaction { get; set; }
    }
}
