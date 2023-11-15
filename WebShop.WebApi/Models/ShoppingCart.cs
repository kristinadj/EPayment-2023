using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.WebApi.Models
{
    [Table("ShoppingCarts", Schema = "dbo")]
    public class ShoppingCart
    {
        [Key]
        public int ShoppingCartId { get; set; }
        public string UserId { get; set; }

        public User? User { get; set; }
        public ICollection<ShoppingCartItem>? ShoppingCartItems { get; set; }

        public ShoppingCart(string userId)
        {
            UserId = userId;
        }
    }
}
