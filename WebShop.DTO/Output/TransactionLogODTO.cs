using WebShop.DTO.Enums;

namespace WebShop.DTO.Output
{
    public class TransactionLogODTO
    {
        public int TransactionLogId { get; set; }
        public int TransactionId { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
