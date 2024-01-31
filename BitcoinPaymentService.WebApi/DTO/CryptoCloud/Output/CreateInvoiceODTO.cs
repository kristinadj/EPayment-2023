using System.Text.Json.Serialization;

namespace BitcoinPaymentService.WebApi.DTO.CryptoCloud.Output
{
    public class CreateInvoiceODTO
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("result")]
        public InvoiceResultODTO Result { get; set; }
    }
}
