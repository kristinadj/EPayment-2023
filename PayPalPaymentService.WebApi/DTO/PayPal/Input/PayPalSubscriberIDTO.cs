using System.Text.Json.Serialization;

namespace PayPalPaymentService.WebApi.DTO.PayPal.Input
{
    public class PayPalSubscriberIDTO
    {
        [JsonPropertyName("name")]
        public PayPalNameIDTO? Name { get; set; }

        [JsonPropertyName("email_address")]
        public string? EmailAddress { get; set; }
    }
}
