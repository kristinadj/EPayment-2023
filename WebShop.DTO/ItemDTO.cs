namespace WebShop.DTO
{
    public class ItemDTO
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }

        public CurrencyDTO? Currency { get; set; }
        public MerchantDTO? Merchant { get; set; }

        public ItemDTO(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
