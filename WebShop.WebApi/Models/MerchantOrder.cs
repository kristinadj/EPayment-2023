using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.WebApi.Models
{
    [Table("OrderMerchants", Schema = "dbo")]
    public class MerchantOrder
    {
        public int MerchantOrderId { get; set; }
        public int MerchantId { get; set; }
        public int OrderId { get; set; }
        public int? InvoiceId { get; set; }

        public Order? Order { get; set; }
        public Merchant? Merchant { get; set; }
        public Invoice? Invoice { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}
