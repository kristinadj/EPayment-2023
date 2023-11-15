using System.ComponentModel.DataAnnotations;

namespace WebShop.DTO.Input
{
    public class ShoppingCartItemIDTO
    {
        [Required]
        public int ShoppingCartId { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
