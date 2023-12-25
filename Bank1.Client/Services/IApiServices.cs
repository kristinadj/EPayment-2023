using Base.DTO.Input;
using Base.DTO.NBS;
using Base.DTO.Shared;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Bank1.Client.Services
{
    public interface IApiServices
    {
        Task<RedirectUrlDTO?> PayTransactionAsync(PayTransactionIDTO payTransactionIDTO);
        Task<string?> GenerateQrCodeAsync(int transactionId);
        Task<string?> GetQrCodeInputAsync(int transactionId);
        Task<QrCodeODTO?> ValdiateQrCodeAsync(int transactionId);
        Task<QrCodeODTO?> ValdiateQrCodeAsync(BankvalidateQrCodeIDTO input);
    }

    public class ApiServices : IApiServices
    {
        private readonly HttpClient _httpClient;

        public ApiServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string?> GenerateQrCodeAsync(int transactionId)
        {
            string? qrCodeSvg = null;

            try
            {
                var response = await _httpClient.GetAsync($"api/Transaction/QrCode/{transactionId}");
                response.EnsureSuccessStatusCode();

                qrCodeSvg = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                // TODO:
            }

            return qrCodeSvg;
        }

        public async Task<string?> GetQrCodeInputAsync(int transactionId)
        {
            string? qrCodeInput = null;

            try
            {
                var response = await _httpClient.GetAsync($"api/Transaction/QrCode/Input/{transactionId}");
                response.EnsureSuccessStatusCode();

                qrCodeInput = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                // TODO:
            }

            return qrCodeInput;
        }

        public async Task<RedirectUrlDTO?> PayTransactionAsync(PayTransactionIDTO payTransactionIDTO)
        {
            RedirectUrlDTO? data = null;

            try
            {
                var content = new StringContent(JsonSerializer.Serialize(payTransactionIDTO), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"api/Transaction", content);
                response.EnsureSuccessStatusCode();

                var tempData = await response.Content.ReadFromJsonAsync<RedirectUrlDTO?>();
                if (tempData != null) { data = tempData; }
            }
            catch (Exception ex)
            {
                // TODO:
            }

            return data;
        }

        public async Task<QrCodeODTO?> ValdiateQrCodeAsync(int transactionId)
        {
            QrCodeODTO? data = null;

            try
            {
                var response = await _httpClient.GetAsync($"api/Transaction/QrCode/Validate/{transactionId}");
                response.EnsureSuccessStatusCode();

                var tempData = await response.Content.ReadFromJsonAsync<QrCodeODTO?>();
                if (tempData != null) { data = tempData; }
            }
            catch (Exception ex)
            {
                // TODO:
            }

            return data;
        }

        public async Task<QrCodeODTO?> ValdiateQrCodeAsync(BankvalidateQrCodeIDTO input)
        {
            QrCodeODTO? data = null;

            try
            {
                var content = new StringContent(JsonSerializer.Serialize(input), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"api/Transaction/QrCode/Validate", content);
                response.EnsureSuccessStatusCode();

                var tempData = await response.Content.ReadFromJsonAsync<QrCodeODTO?>();
                if (tempData != null) { data = tempData; }
            }
            catch (Exception ex)
            {
                // TODO:
            }

            return data;
        }
    }
}
