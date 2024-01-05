namespace Base.DTO.Input
{
    public class ProductIDTO
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }

        public ProductIDTO(string name, string type, string description, string category)
        {
            Name = name;
            Type = type;
            Description = description;
            Category = category;
        }
    }
}
