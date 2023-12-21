using System.Text.Json.Serialization;

namespace PayPalPaymentService.WebApi.DTO.PayPal.Input
{
    public class ExperienceContextIDTO
    {
        [JsonPropertyName("payment_method_preference")]
        public string PaymentMethodPreference { get; set; } = "IMMEDIATE_PAYMENT_REQUIRED";

        [JsonPropertyName("landing_page")]
        public string LandingPage { get; set; } = "LOGIN";

        [JsonPropertyName("user_action")]
        public string UserAction { get; set; } = "PAY_NOW";

        [JsonPropertyName("return_url")]
        public string ReturnUrl { get; set; } = string.Empty;

        [JsonPropertyName("cancel_url")]
        public string CancelUrl { get; set; } = string.Empty;
    }
}
