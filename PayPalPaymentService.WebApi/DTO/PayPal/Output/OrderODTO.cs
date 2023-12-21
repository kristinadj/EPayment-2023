using System.Text.Json.Serialization;

namespace PayPalPaymentService.WebApi.DTO.PayPal.Output
{
    public class OrderODTO
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("payment_source")]
        public PaymentSourceODTO? PaymentSource { get; set; }

        [JsonPropertyName("links")]
        public List<LinkODTO> Links { get; set; } = new();
    }
}
