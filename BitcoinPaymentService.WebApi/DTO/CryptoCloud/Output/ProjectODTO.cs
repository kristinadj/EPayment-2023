using System.Text.Json.Serialization;

namespace BitcoinPaymentService.WebApi.DTO.CryptoCloud.Output
{
    public class ProjectODTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("fail")]
        public string Fail { get; set; }

        [JsonPropertyName("success")]
        public string Success { get; set; }

        [JsonPropertyName("logo")]
        public string Logo { get; set; }
    }
}
