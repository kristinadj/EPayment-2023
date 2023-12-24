using Bank1.WebApi.AppSettings;
using Bank1.WebApi.DTO.NBS;
using Microsoft.Extensions.Options;
using Net.Codecrete.QrCodeGenerator;
using System.Drawing;
using System.Text.Json;

namespace Bank1.WebApi.Services
{
    public interface INbsClient
    {
        Task<string> GenerateQrCodeAsync(QrCodeGenDTO qrCodeGenIDTO);
        Task<QrCodeValidateODTO?> ValdiateQrCodeAsync(string qrCodeToValidate);
    }

    public class NbsClient : INbsClient
    {
        private readonly HttpClient _httpClient;

        public NbsClient(IOptions<BankSettings> bankSettings)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(bankSettings.Value.NbsUrl)
            };
        }

        public async Task<string> GenerateQrCodeAsync(QrCodeGenDTO qrCodeGenIDTO)
        {
            var response = await _httpClient.PostAsJsonAsync("/gen?lang=en", qrCodeGenIDTO);
            if (!response.IsSuccessStatusCode)
            {
                return "";
            }

            var bytes = await response.Content.ReadAsByteArrayAsync();
            var qrCode = QrCode.EncodeBinary(bytes, QrCode.Ecc.Medium);

            return qrCode.ToSvgString(2);
        }

        public async Task<QrCodeValidateODTO?> ValdiateQrCodeAsync(string qrCodeToValidate)
        {
            var httpContent = new StringContent(qrCodeToValidate);
            var response = await _httpClient.PostAsync("/validate?lang=en", httpContent);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var validateODTO = JsonSerializer.Deserialize<QrCodeValidateODTO>(responseContent);

            if (validateODTO == null) return null;

            return validateODTO;
        }
    }
}
