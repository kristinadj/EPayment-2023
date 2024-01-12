using System.Text.Json.Serialization;

namespace PayPalPaymentService.WebApi.DTO.PayPal.Output
{
    public class CreateSubscriptionODTO
    {
        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("status_update_time")]
        public DateTime StatusUpdateTime { get; set; }

        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("plan_id")]
        public string? PlanId { get; set; }

        [JsonPropertyName("start_time")]
        public DateTime StartTime { get; set; }

        [JsonPropertyName("quantity")]
        public string? Quantity { get; set; }

        [JsonPropertyName("subscriber")]
        public SubscriberODTO? Subscriber { get; set; }

        [JsonPropertyName("create_time")]
        public DateTime CreateTime { get; set; }

        [JsonPropertyName("plan_overridden")]
        public bool PlanOverridden { get; set; }

        [JsonPropertyName("links")]
        public List<LinkODTO> Links { get; set; }
    }
}
