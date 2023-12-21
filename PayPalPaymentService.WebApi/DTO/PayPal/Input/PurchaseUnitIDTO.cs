using System.Text.Json.Serialization;

namespace PayPalPaymentService.WebApi.DTO.PayPal.Input
{
    public class PurchaseUnitIDTO
    {
        [JsonPropertyName("reference_id")]
        public string ReferenceId { get; set; } = string.Empty;

        [JsonPropertyName("amount")]
        public AmountIDTO? Amount { get; set; }
    }
}
