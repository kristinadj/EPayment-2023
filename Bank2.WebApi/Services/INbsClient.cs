using Bank2.WebApi.AppSettings;
using Base.DTO.NBS;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Bank2.WebApi.Services
{
    public interface INbsClient
    {
        Task<QrCodeODTO?> GenerateQrCodeAsync(string qrCodeGenIDTO);
        Task<QrCodeODTO?> ValdiateQrCodeAsync(string qrCodeToValidate);
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

        public async Task<QrCodeODTO?> GenerateQrCodeAsync(string qrCodeGenIDTO)
        {
            QrCodeODTO? qrCodeODTO = null;

            var stringContent = new StringContent(qrCodeGenIDTO);
            var response = await _httpClient.PostAsync("generate", stringContent);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                qrCodeODTO = JsonSerializer.Deserialize<QrCodeODTO>(responseContent);
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"NBS API for generating QR code responded with status code {response.StatusCode} and message {responseContent}");
            }

            return qrCodeODTO;
        }

        public async Task<QrCodeODTO?> ValdiateQrCodeAsync(string qrCodeToValidate)
        {
            var httpContent = new StringContent(qrCodeToValidate);
            var response = await _httpClient.PostAsync("validate", httpContent);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var validateODTO = JsonSerializer.Deserialize<QrCodeODTO>(responseContent);
            return validateODTO;
        }
    }
}
