using System.Text.Json.Serialization;

namespace PayPalPaymentService.WebApi.DTO.PayPal.Output
{
    public class SubscriberODTO
    {
        [JsonPropertyName("email_address")]
        public string? EmailAddress { get; set; }

        [JsonPropertyName("name")]
        public NameODTO? Name { get; set; }
    }
}
