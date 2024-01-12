using System.Text.Json.Serialization;

namespace PayPalPaymentService.WebApi.DTO.PayPal.Input
{
    public class ApplicationContextIDTO
    {
        [JsonPropertyName("brand_name")]
        public string? BrandName { get; set; }

        [JsonPropertyName("user_action")]
        public string? UserAction { get; set; }

        [JsonPropertyName("payment_method")]
        public PaymentMethodIDTO? PaymentMethod { get; set; }

        [JsonPropertyName("return_url")]
        public string? ReturnUrl { get; set; }

        [JsonPropertyName("cancel_url")]
        public string? CancelUrl { get; set; }
    }
}
