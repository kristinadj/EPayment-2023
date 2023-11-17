namespace WebShop.DTO.Output
{
    public class OrderItemODTO
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public int CurrencyId { get; set; }
        public ItemODTO? Item { get; set; }
        public CurrencyODTO? Currency { get; set; }
    }
}
