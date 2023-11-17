using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebShop.DTO.Enums;

namespace WebShop.WebApi.Models
{
    [Table("OrderLogs", Schema = "dbo")]
    public class OrderLog
    {
        [Key]
        public int OrderLogId { get; set; }
        public int OrderId { get; set; }

        [Column(TypeName = "nvarchar(24)")]
        public OrderStatus OrderStatus { get; set; }
        public DateTime Timestamp { get; set; }

        public Order? Order { get; set; }   
    }
}
