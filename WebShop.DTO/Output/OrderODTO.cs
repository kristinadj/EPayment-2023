using WebShop.DTO.Enums;

namespace WebShop.DTO.Output
{
    public class OrderODTO
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public MerchantODTO? Merchant { get; set; }
        public InvoiceODTO? Invoice { get; set; }
        public List<OrderItemODTO>? OrderItems { get; set; }
        public List<OrderLogODTO>? OrderLogs { get; set; }

        public OrderODTO(string userId)
        {
            UserId = userId;
        }
    }
}
