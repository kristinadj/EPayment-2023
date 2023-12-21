using System.Text.Json.Serialization;

namespace PayPalPaymentService.WebApi.DTO.PayPal.Output
{
    public class AuthODTO
    {
        [JsonPropertyName("scope")]
        public string Scope { get; set; } = string.Empty;

        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("token_type")]
        public string TokentType { get; set; } = string.Empty;

        [JsonPropertyName("app_id")]
        public string AppId { get; set; } = string.Empty;

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("supported_authn_schemes")]
        public List<string> SupportedAuthnSchemes { get; set; } = new();

        [JsonPropertyName("nonce")]
        public string Nonce { get; set; } = string.Empty;

        [JsonPropertyName("client_metadata")]
        public ClientMetadataODTO? ClientMetadata { get; set; }
    }
}
