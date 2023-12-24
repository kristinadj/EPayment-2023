using System.Text.Json.Serialization;

namespace Bank1.WebApi.DTO.NBS
{
    public class QrCodeValidateODTO
    {
        [JsonPropertyName("s")]
        public QrCodeODTO? Status { get; set; }

        [JsonPropertyName("t")]
        public string Text { get; set; } = string.Empty;

        [JsonPropertyName("n")]
        public QrCodeGenDTO? Data { get; set; }
    }
}
