using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebShop.DTO.Enums;

namespace WebShop.WebApi.Models
{
    [Table("Orders", Schema = "dbo")]
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public string UserId { get; set; }

        [Column(TypeName = "nvarchar(24)")]
        public OrderStatus OrderStatus { get; set; }
        public DateTime CreatedTimestamp { get; set; }

        public User? User { get; set; }
        public ICollection<MerchantOrder>? MerchantOrders { get; set; }
        public ICollection<OrderLog>? OrderLogs { get; set; }

        public Order(string userId)
        {
            UserId = userId;
        }
    }
}
