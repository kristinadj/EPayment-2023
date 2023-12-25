using System.Text.Json.Serialization;

namespace Base.DTO.NBS
{
    public class QrCodeBaseODTO
    {
        [JsonPropertyName("s")]
        public QrCodeStatusODTO? Status { get; set; }
    }
}
