using System.Text.Json.Serialization;

namespace PayPalPaymentService.WebApi.DTO.PayPal.Output
{
    public class PricingSchemeODTO
    {

        [JsonPropertyName("version")]
        public int Version { get; set; }

        [JsonPropertyName("fixed_price")]
        public FixedPriceODTO? FixedPrice { get; set; }

        [JsonPropertyName("create_time")]
        public DateTime CreateTime { get; set; }

        [JsonPropertyName("update_time")]
        public DateTime UpdateTime { get; set; }
    }
}
