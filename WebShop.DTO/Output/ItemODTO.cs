namespace WebShop.DTO.Output
{
    public class ItemODTO
    {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }

        public CurrencyODTO? Currency { get; set; }
        public MerchantODTO? Merchant { get; set; }

        public ItemODTO(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
