namespace WebShop.DTO.Output
{
    public class ShoppingCartODTO
    {
        public int ShoppingCartId { get; set; }

        public List<ShoppingCartItemODTO>? ShoppingCartItems { get; set; }

    }
}
