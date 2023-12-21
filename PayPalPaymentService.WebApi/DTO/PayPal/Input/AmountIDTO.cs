using System.Text.Json.Serialization;

namespace PayPalPaymentService.WebApi.DTO.PayPal.Input
{
    public class AmountIDTO
    {
        [JsonPropertyName("currency_code")]
        public string CurrencyCode { get; set; } = string.Empty;

        [JsonPropertyName("value")]
        public string Value { get; set; } = string.Empty;
    }
}
