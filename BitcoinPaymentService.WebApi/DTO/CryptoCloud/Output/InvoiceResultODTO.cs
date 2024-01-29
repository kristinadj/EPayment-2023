using System.Text.Json.Serialization;

namespace BitcoinPaymentService.WebApi.DTO.CryptoCloud.Output
{
    public class InvoiceResultODTO
    {
        [JsonPropertyName("uuid")]
        public string Uuid { get; set; }

        [JsonPropertyName("created")]
        public DateTime Created { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("expiry_date")]
        public DateTime ExpiryDate { get; set; }

        [JsonPropertyName("side_commission")]
        public string SideCommission { get; set; }

        [JsonPropertyName("side_commission_service")]
        public string SideCommissionService { get; set; }

        [JsonPropertyName("type_payments")]
        public string TypePayments { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("amount_usd")]
        public decimal AmountUsd { get; set; }

        [JsonPropertyName("amount_in_fiat")]
        public decimal AmountInFiat { get; set; }

        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }

        [JsonPropertyName("fee_usd")]
        public decimal FeeUsd { get; set; }

        [JsonPropertyName("service_fee")]
        public decimal ServiceFee { get; set; }

        [JsonPropertyName("service_fee_usd")]
        public decimal ServiceFeeUsd { get; set; }

        [JsonPropertyName("fiat_currency")]
        public string FiatCurrency { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("is_email_required")]
        public bool IsEmailRequired { get; set; }

        [JsonPropertyName("link")]
        public string Link { get; set; }

        [JsonPropertyName("invoice_id")]
        public object InvoiceId { get; set; }

        [JsonPropertyName("currency")]
        public CurrencyODTO Currency { get; set; }

        [JsonPropertyName("project")]
        public ProjectODTO Project { get; set; }

        [JsonPropertyName("test_mode")]
        public bool TestMode { get; set; }
    }
}
