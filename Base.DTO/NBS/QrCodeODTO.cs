using System.Text.Json.Serialization;

namespace Base.DTO.NBS
{
    public class QrCodeODTO
    {
        [JsonPropertyName("s")]
        public QrCodeStatusODTO? Status { get; set; }

        [JsonPropertyName("t")]
        public string Text { get; set; } = string.Empty;

        [JsonPropertyName("n")]
        public QrCodeGenDTO? Data { get; set; }

        [JsonPropertyName("i")]
        public string Base64QrCode { get; set; } = string.Empty;
    }
}
