using System.Text.Json.Serialization;

namespace PayPalPaymentService.WebApi.DTO.PayPal.Input
{
    public class PaymentMethodIDTO
    {
        [JsonPropertyName("payer_selected")]
        public string? PayerSelected { get; set; }

        [JsonPropertyName("payee_preferred")]
        public string? PayeePreferred { get; set; }
    }
}
