using System.Text.Json.Serialization;

namespace PayPalPaymentService.WebApi.DTO.PayPal.Input
{
    public class PayPalProductIDTO
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("category")]
        public string Category { get; set; }

        public PayPalProductIDTO(string name, string type, string description, string category)
        {
            Name = name;
            Type = type;
            Description = description;
            Category = category;
        }
    }
}
