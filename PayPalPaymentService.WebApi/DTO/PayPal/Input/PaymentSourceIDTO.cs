using System.Text.Json.Serialization;

namespace PayPalPaymentService.WebApi.DTO.PayPal.Input
{
    public class PaymentSourceIDTO
    {
        [JsonPropertyName("paypal")]
        public PaypalIDTO? Paypal { get; set; }
    }
}
