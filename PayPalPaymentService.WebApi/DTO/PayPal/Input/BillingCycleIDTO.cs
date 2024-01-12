using System.Text.Json.Serialization;

namespace PayPalPaymentService.WebApi.DTO.PayPal.Input
{
    public class BillingCycleIDTO
    {
        [JsonPropertyName("frequency")]
        public FrequencyIDTO? Frequency { get; set; }

        [JsonPropertyName("tenure_type")]
        public string? TenureType { get; set; }

        [JsonPropertyName("sequence")]
        public int Sequence { get; set; }

        [JsonPropertyName("total_cycles")]
        public int TotalCycles { get; set; }

        [JsonPropertyName("pricing_scheme")]
        public PricingSchemeIDTO? PricingScheme { get; set; }
    }
}
