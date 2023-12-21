using System.Text.Json.Serialization;

namespace PayPalPaymentService.WebApi.DTO.PayPal.Input
{
    public class OrderIDTO
    {
        [JsonPropertyName("intent")]
        public string Intent { get; set; } = "CAPTURE";

        [JsonPropertyName("purchase_units")]
        public List<PurchaseUnitIDTO> PurchaseUnits { get; set; } = new();

        [JsonPropertyName("payment_source")]
        public PaymentSourceIDTO? PaymentSource { get; set; }
    }
}
