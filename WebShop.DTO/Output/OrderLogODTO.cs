using WebShop.DTO.Enums;

namespace WebShop.DTO.Output
{
    public class OrderLogODTO
    {
        public int OrderLogId { get; set; }
        public int OrderId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
