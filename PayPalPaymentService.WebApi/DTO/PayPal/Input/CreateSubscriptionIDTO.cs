using System.Text.Json.Serialization;

namespace PayPalPaymentService.WebApi.DTO.PayPal.Input
{
    public class CreateSubscriptionIDTO
    {

        [JsonPropertyName("plan_id")]
        public string? PlanId { get; set; }

        [JsonPropertyName("subscriber")]
        public PayPalSubscriberIDTO? Subscriber { get; set; }

        [JsonPropertyName("application_context")]
        public ApplicationContextIDTO? ApplicationContext { get; set; }
    }
}
