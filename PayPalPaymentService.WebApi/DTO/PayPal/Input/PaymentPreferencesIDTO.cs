using System.Text.Json.Serialization;

namespace PayPalPaymentService.WebApi.DTO.PayPal.Input
{
    public class PaymentPreferencesIDTO
    {
        [JsonPropertyName("auto_bill_outstanding")]
        public bool AutoBillOutstanding { get; set; }

        [JsonPropertyName("payment_failure_threshold")]
        public int PaymentFailureThreshold { get; set; }
    }
}
