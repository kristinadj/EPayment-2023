using System.Text.Json.Serialization;

namespace PayPalPaymentService.WebApi.DTO.PayPal.Input
{
    public class FixedPriceIDTO
    {
        [JsonPropertyName("value")]
        public string? Value { get; set; }

        [JsonPropertyName("currency_code")]
        public string? CurrencyCode { get; set; }
    }
}
