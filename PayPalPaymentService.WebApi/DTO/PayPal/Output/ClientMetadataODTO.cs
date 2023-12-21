using System.Text.Json.Serialization;

namespace PayPalPaymentService.WebApi.DTO.PayPal.Output
{
    public class ClientMetadataODTO
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("logo_uri")]
        public string LogoUri { get; set; } = string.Empty;

        [JsonPropertyName("scopes")]
        public List<string> Scopes { get; set; } = new();

        [JsonPropertyName("ui_type")]
        public string UIType { get; set; } = string.Empty;

    }
}
