using System.Text.Json.Serialization;

namespace PayPalPaymentService.WebApi.DTO.PayPal.Output
{
    public class PaymentPreferencesODTO
    {
        [JsonPropertyName("service_type")]
        public string? ServiceType { get; set; }

        [JsonPropertyName("auto_bill_outstanding")]
        public bool AutoBillOutstanding { get; set; }

        [JsonPropertyName("setup_fee")]
        public SetupFeeODTO? SetupFee { get; set; }

        [JsonPropertyName("setup_fee_failure_action")]
        public string? SetupFeeFailureAction { get; set; }

        [JsonPropertyName("payment_failure_threshold")]
        public int PaymentFailureThreshold { get; set; }
    }
}
