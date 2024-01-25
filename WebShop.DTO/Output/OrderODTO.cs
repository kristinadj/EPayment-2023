using WebShop.DTO.Enums;

namespace WebShop.DTO.Output
{
    public class OrderODTO
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public List<MerchantOrderODTO>? MerchantOrders { get; set; }
        public List<OrderLogODTO>? OrderLogs { get; set; }

        public OrderODTO(string userId)
        {
            UserId = userId;
        }
    }
}
