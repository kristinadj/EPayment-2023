using Base.DTO.Input;
using Base.DTO.Output;
using System.Text;
using System.Text.Json;

namespace WebShop.WebApi.Services
{
    public interface IPspApiHttpClient
    {
        Task<T?> GetAsync<T>(string requestUri);
        Task<T?> PostAsync<T>(string requestUri, T requestBody);
        Task<bool> PutAsync(string requestUri);
        Task<bool> PutAsync<T>(string requestUri, T requestBody);
        Task<PaymentInstructionsODTO?> PostAsync(string requestUri, PaymentRequestIDTO requestBody);
    }

    public class PspApiHttpClient : IPspApiHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public PspApiHttpClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("PspAPI");
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<T?> GetAsync<T>(string requestUri)
        {
            var response = await _httpClient.GetAsync($"api/PSP/{requestUri}");

            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(content, _jsonSerializerOptions);
        }

        public async Task<T?> PostAsync<T>(string requestUri, T requestBody)
        {
            var requestContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"api/PSP/{requestUri}", requestContent);

            if (!response.IsSuccessStatusCode) return default;

            var content = await response.Content.ReadAsStringAsync();
            if (content == null) return default;

            return JsonSerializer.Deserialize<T>(content, _jsonSerializerOptions);
        }

        public async Task<bool> PutAsync(string requestUri)
        {
            var response = await _httpClient.PutAsync($"api/PSP/{requestUri}", null);

            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public async Task<bool> PutAsync<T>(string requestUri, T requestBody)
        {
            var requestContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/PSP/{requestUri}", requestContent);

            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public async Task<PaymentInstructionsODTO?> PostAsync(string requestUri, PaymentRequestIDTO requestBody)
        {
            var requestContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"api/PSP/{requestUri}", requestContent);

            if (!response.IsSuccessStatusCode) return default;

            var content = await response.Content.ReadAsStringAsync();
            if (content == null) return default;

            return JsonSerializer.Deserialize<PaymentInstructionsODTO>(content, _jsonSerializerOptions);
        }
    }
}
