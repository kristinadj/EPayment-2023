using System.Text.Json.Serialization;

namespace PayPalPaymentService.WebApi.DTO.PayPal.Input
{
    public class PaypalIDTO
    {
        [JsonPropertyName("experience_context")]
        public ExperienceContextIDTO? ExperienceContext { get; set; }
    }
}
