using System.Text.Json.Serialization;

namespace PayPalPaymentService.WebApi.DTO.PayPal.Output
{
    public class CreatePlanODTO
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("product_id")]
        public string? ProductId { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("usage_type")]
        public string? UsageType { get; set; }

        [JsonPropertyName("billing_cycles")]
        public List<BillingCycleODTO>? BillingCycles { get; set; }

        [JsonPropertyName("payment_preferences")]
        public PaymentPreferencesODTO? PaymentPreferences { get; set; }

        [JsonPropertyName("quantity_supported")]
        public bool QuantitySupported { get; set; }

        [JsonPropertyName("create_time")]
        public DateTime CreateTime { get; set; }

        [JsonPropertyName("update_time")]
        public DateTime UpdateTime { get; set; }

        [JsonPropertyName("links")]
        public List<LinkODTO>? Links { get; set; }
    }
}
