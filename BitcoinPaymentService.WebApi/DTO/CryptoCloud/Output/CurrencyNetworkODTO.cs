using System.Text.Json.Serialization;

namespace BitcoinPaymentService.WebApi.DTO.CryptoCloud.Output
{
    public class CurrencyNetworkODTO
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("fullname")]
        public string FullName { get; set; }
    }
}
