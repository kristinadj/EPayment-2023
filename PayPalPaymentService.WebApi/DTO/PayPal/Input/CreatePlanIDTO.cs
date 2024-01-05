using System.Text.Json.Serialization;

namespace PayPalPaymentService.WebApi.DTO.PayPal.Input
{
    public class CreatePlanIDTO
    {
        [JsonPropertyName("product_id")]
        public string? ProductId { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("billing_cycles")]
        public List<BillingCycleIDTO>? BillingCycles { get; set; } = new();

        [JsonPropertyName("payment_preferences")]
        public PaymentPreferencesIDTO? PaymentPreferences { get; set; }
    }
}
