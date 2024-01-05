using System.Text.Json.Serialization;

namespace PayPalPaymentService.WebApi.DTO.PayPal.Output
{
    public class BillingCycleODTO
    {
        [JsonPropertyName("pricing_scheme")]
        public PricingSchemeODTO? PricingScheme { get; set; }

        [JsonPropertyName("frequency")]
        public FrequencyODTO? Frequency { get; set; }

        [JsonPropertyName("tenure_type")]
        public string? TenureType { get; set; }

        [JsonPropertyName("sequence")]
        public int Sequence { get; set; }

        [JsonPropertyName("total_cycles")]
        public int TotalCycles { get; set; }
    }
}
