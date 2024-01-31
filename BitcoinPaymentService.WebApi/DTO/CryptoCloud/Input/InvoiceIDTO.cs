using System.Text.Json.Serialization;

namespace BitcoinPaymentService.WebApi.DTO.CryptoCloud.Input
{
    public class InvoiceIDTO
    {
        [JsonPropertyName("shop_id")]
        public string ShopId {  get; set; } = string.Empty;

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("currency")]
        public string CurrencyCode { get; set; } = string.Empty;

        [JsonPropertyName("order_id")]
        public string OrderId { get; set; } = string.Empty;
    }
}
