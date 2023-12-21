using System.Text.Json.Serialization;

namespace PayPalPaymentService.WebApi.DTO.PayPal.Output
{
    public class PaymentSourceODTO
    {
        [JsonPropertyName("paypal")]
        public PayPalODTO? PayPal { get; set; }
    }
}
