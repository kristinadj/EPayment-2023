using System.ComponentModel.DataAnnotations.Schema;
using WebShop.DTO.Enums;

namespace WebShop.DTO.Output
{
    public class TransactionODTO
    {
        public int TransactionId { get; set; }
        public int InvoiceId { get; set; }
        public DateTime CreatedTimestamp { get; set; }

        [Column(TypeName = "nvarchar(24)")]
        public TransactionStatus TransactionStatus { get; set; }
        public PaymentMethodODTO? PaymentMethod { get; set; }
        public List<TransactionLogODTO>? TransactionLogs { get; set; }
    }
}
