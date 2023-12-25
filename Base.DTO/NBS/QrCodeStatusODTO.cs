using System.Text.Json.Serialization;

namespace Base.DTO.NBS
{
    public class QrCodeStatusODTO
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("desc")]
        public string Description { get; set; } = string.Empty;
    }
}
