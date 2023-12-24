using System.Text.Json.Serialization;

namespace Bank1.WebApi.DTO.NBS
{
    public class ErrorODTO
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("desc")]
        public string Description { get; set; } = string.Empty;
    }
}
