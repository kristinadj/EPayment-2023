using System.Text.Json.Serialization;

namespace PayPalPaymentService.WebApi.DTO.PayPal.Input
{
    public class FrequencyIDTO
    {
        [JsonPropertyName("interval_unit")]
        public string? IntervalUnit { get; set; }

        [JsonPropertyName("interval_count")]
        public int IntervalCount { get; set; }
    }
}
