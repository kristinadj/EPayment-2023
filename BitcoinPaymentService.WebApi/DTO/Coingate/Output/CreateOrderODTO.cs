using System.Text.Json.Serialization;

namespace BitcoinPaymentService.WebApi.DTO.Coingate.Output
{
    public class CreateOrderODTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("do_not_convert")]
        public bool DoNotConvert { get; set; }

        [JsonPropertyName("orderable_type")]
        public string OrderableType { get; set; }

        [JsonPropertyName("orderable_id")]
        public int OrderableId { get; set; }

        [JsonPropertyName("price_currency")]
        public string PriceCurrency { get; set; }

        [JsonPropertyName("price_amount")]
        public string PriceAmount { get; set; }

        [JsonPropertyName("lightning_network")]
        public bool LightningNetwork { get; set; }

        [JsonPropertyName("receive_currency")]
        public string ReceiveCurrency { get; set; }

        [JsonPropertyName("receive_amount")]
        public string ReceiveAmount { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("order_id")]
        public string OrderId { get; set; }

        [JsonPropertyName("payment_url")]
        public string PaymentUrl { get; set; }

        [JsonPropertyName("underpaid_amount")]
        public string UnderpaidAmount { get; set; }

        [JsonPropertyName("overpaid_amount")]
        public string OverpaidAmount { get; set; }

        [JsonPropertyName("is_refundable")]
        public bool IsRefundable { get; set; }

        [JsonPropertyName("refunds")]
        public object[] Refunds { get; set; }

        [JsonPropertyName("voids")]
        public object[] Voids { get; set; }

        [JsonPropertyName("fees")]
        public object[] Fees { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("blockchain_transactions")]
        public object[] BlockchainTransactions { get; set; }
    }
}
