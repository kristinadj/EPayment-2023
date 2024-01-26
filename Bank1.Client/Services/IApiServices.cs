using Base.DTO.Input;
using Base.DTO.NBS;
using Base.DTO.Shared;
using System.Text;
using System.Text.Json;

namespace Bank1.Client.Services
{
    public interface IApiServices
    {
        Task<RedirectUrlDTO?> PayTransactionAsync(PayTransactionIDTO payTransactionIDTO);
        Task<RedirectUrlDTO?> UpdateTransactionFailedAsync(int transactionId);
        Task<string?> GenerateQrCodeAsync(int transactionId);
        Task<string?> GetQrCodeInputAsync(int transactionId);
        Task<QrCodeODTO?> ValdiateQrCodeAsync(int transactionId);
        Task<QrCodeODTO?> ValdiateQrCodeAsync(BankvalidateQrCodeIDTO input);

    }

    public class ApiServices : IApiServices
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public ApiServices(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("BankAPI");
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<string?> GenerateQrCodeAsync(int transactionId)
        {
            var response = await _httpClient.GetAsync($"api/Transaction/QrCode/{transactionId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return null;
        }

        public async Task<string?> GetQrCodeInputAsync(int transactionId)
        {
            var response = await _httpClient.GetAsync($"api/Transaction/QrCode/Input/{transactionId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return null;
        }

        public async Task<RedirectUrlDTO?> PayTransactionAsync(PayTransactionIDTO payTransactionIDTO)
        {
            RedirectUrlDTO? data = null;

            var requestContent = new StringContent(JsonSerializer.Serialize(payTransactionIDTO), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/Transaction", requestContent);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    var tempData = JsonSerializer.Deserialize<RedirectUrlDTO>(content, _jsonSerializerOptions);
                    if (tempData != null) { data = tempData; }
                }
            }

            return data;
        }

        public async Task<RedirectUrlDTO?> UpdateTransactionFailedAsync(int transactionId)
        {
            RedirectUrlDTO? data = null;

            var response = await _httpClient.PutAsync($"api/Transaction/Failed/{transactionId}", null);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    var tempData = JsonSerializer.Deserialize<RedirectUrlDTO>(content, _jsonSerializerOptions);
                    if (tempData != null) { data = tempData; }
                }
            }

            return data;
        }

        public async Task<QrCodeODTO?> ValdiateQrCodeAsync(int transactionId)
        {
            QrCodeODTO? data = null;

            var response = await _httpClient.GetAsync($"api/Transaction/QrCode/Validate/{transactionId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    var tempData = JsonSerializer.Deserialize<QrCodeODTO>(content, _jsonSerializerOptions);
                    if (tempData != null) { data = tempData; }
                }
            }

            return data;
        }

        public async Task<QrCodeODTO?> ValdiateQrCodeAsync(BankvalidateQrCodeIDTO input)
        {
            QrCodeODTO? data = null;

            var requestContent = new StringContent(JsonSerializer.Serialize(input), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"api/Transaction/QrCode/Validate", requestContent);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    var tempData = JsonSerializer.Deserialize<QrCodeODTO>(content, _jsonSerializerOptions);
                    if (tempData != null) { data = tempData; }
                }
            }

            return data;
        }
    }
}
