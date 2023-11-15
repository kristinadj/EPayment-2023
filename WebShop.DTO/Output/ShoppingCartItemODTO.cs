namespace WebShop.DTO.Output
{
    public class ShoppingCartItemODTO
    {
        public int ShoppingCartItemId { get; set; }
        public int Quantity { get; set; }
        public ItemODTO? Item { get; set; }
    }
}
