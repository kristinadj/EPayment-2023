using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebShop.WebApi.Enums;

namespace WebShop.WebApi.Models
{
    [Table("Orders", Schema = "dbo")]
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public int InvoiceId { get; set; }

        public User? User { get; set; }
        public Invoice? Invoice { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
        public ICollection<OrderLog>? OrderLogs { get; set; }

        public Order(string userId)
        {
            UserId = userId;
        }
    }
}
