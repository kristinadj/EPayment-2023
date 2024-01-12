using System.Text.Json.Serialization;

namespace PayPalPaymentService.WebApi.DTO.PayPal.Input
{
    public class PricingSchemeIDTO
    {
        [JsonPropertyName("fixed_price")]
        public FixedPriceIDTO? FixedPrice { get; set; }
    }
}
