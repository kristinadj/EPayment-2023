using System.Text.Json.Serialization;

namespace BitcoinPaymentService.WebApi.DTO.CryptoCloud.Output
{
    public class CurrencyODTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("fullcode")]
        public string Fullcode { get; set; }

        [JsonPropertyName("network")]
        public CurrencyNetworkODTO Network { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("is_email_required")]
        public bool IsEmailRequired { get; set; }

        [JsonPropertyName("stablecoin")]
        public bool Stablecoin { get; set; }

        [JsonPropertyName("icon_base")]
        public string IconBase { get; set; }

        [JsonPropertyName("icon_network")]
        public string IconNetwork { get; set; }

        [JsonPropertyName("icon_qr")]
        public string IconQr { get; set; }

        [JsonPropertyName("order")]
        public int Order { get; set; }
    }
}
