using System.Text.Json.Serialization;

namespace Bank1.WebApi.DTO.NBS
{
    public class QrCodeODTO
    {
        [JsonPropertyName("s")]
        public ErrorODTO? ErrorODTO { get; set; }
    }
}
