using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.WebApi.Models
{
    [Table("ShoppingCartItems", Schema = "dbo")]
    public class ShoppingCartItem
    {
        [Key]
        public int ShoppingCartItemId { get; set; }
        public int ShoppingCartId { get; set; } 
        public int ItemId { get; set; }
        public int Quantity { get; set; }

        public ShoppingCart? ShoppingCart { get; set; }
        public Item? Item { get; set; }
    }
}
