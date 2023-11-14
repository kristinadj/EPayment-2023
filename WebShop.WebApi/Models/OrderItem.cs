using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.WebApi.Models
{
    [Table("OrderItems", Schema = "dbo")]
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public int CurrencyId { get; set; }

        public Order? Order { get; set; }
        public Item? Item { get; set; }
        public Currency? Currency { get; set; }
    }
}
