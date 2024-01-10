using System.Text.Json.Serialization;

namespace PayPalPaymentService.WebApi.DTO.PayPal.Input
{
    public class CancelSubscriptionIDTO
    {
        [JsonPropertyName("reason")]
        public string? Reason { get; set; } = "Other";
    }
}
