using Microsoft.Extensions.Options;
using PayPalPaymentService.WebApi.AppSettings;
using PayPalPaymentService.WebApi.DTO.PayPal.Input;
using PayPalPaymentService.WebApi.DTO.PayPal.Output;
using System.Collections;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text;
using System.Text.Json;

namespace PayPalPaymentService.WebApi.Services
{
    public interface IPayPalClientService
    {
        Task<string?> GenerateAccessTokenAsync(string clientId, string clientSecret);
        Task<OrderODTO?> CreateOrderAsync(string token, OrderIDTO order);
        Task<PayPalProductODTO?> CreateProductAsync(string token, PayPalProductIDTO product);
        Task<CreatePlanODTO?> CreatePlanAsync(string token, CreatePlanIDTO plan);
        Task<CreateSubscriptionODTO?> CreateSubscriptionAsync(string token, CreateSubscriptionIDTO subscription);
    }

    public class PayPalClientService : IPayPalClientService
    {
        private readonly HttpClient _httpClient;

        public PayPalClientService(IOptions<PayPalSettings> payPalSettings)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(payPalSettings.Value.Url)
            };
        }

        public async Task<string?> GenerateAccessTokenAsync(string clientId, string clientSecret)
        {
            var byteArray = new UTF8Encoding().GetBytes($"{clientId}:{clientSecret}");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var formData = new List<KeyValuePair<string, string>>
            {
                new("grant_type", "client_credentials")
            };

            var content = new FormUrlEncodedContent(formData);

            var response = await _httpClient.PostAsync("/v1/oauth2/token", content);
            response.EnsureSuccessStatusCode();

            var stringContent = await response.Content.ReadAsStringAsync();
            var authResponse = JsonSerializer.Deserialize<AuthODTO>(stringContent);

            if (authResponse == null) return null;

            return authResponse.AccessToken;
        }

        public async Task<OrderODTO?> CreateOrderAsync(string token, OrderIDTO order)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync("/v2/checkout/orders", order);
            response.EnsureSuccessStatusCode();

            var stringContent = await response.Content.ReadAsStringAsync();
            var orderResponse = JsonSerializer.Deserialize<OrderODTO>(stringContent);

            if (orderResponse == null) return null;

            return orderResponse;
        }

        public async Task<PayPalProductODTO?> CreateProductAsync(string token, PayPalProductIDTO product)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync("/v1/catalogs/products", product);
            response.EnsureSuccessStatusCode();

            var stringContent = await response.Content.ReadAsStringAsync();
            var productResponse = JsonSerializer.Deserialize<PayPalProductODTO>(stringContent);

            if (productResponse == null) return null;

            return productResponse;
        }

        public async Task<CreatePlanODTO?> CreatePlanAsync(string token, CreatePlanIDTO plan)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync("/v1/billing/plans", plan);
            response.EnsureSuccessStatusCode();

            var stringContent = await response.Content.ReadAsStringAsync();
            var planResponse = JsonSerializer.Deserialize<CreatePlanODTO>(stringContent);

            if (planResponse == null) return null;

            return planResponse;
        }

        public async Task<CreateSubscriptionODTO?> CreateSubscriptionAsync(string token, CreateSubscriptionIDTO subscription)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync("/v1/billing/subscriptions", subscription);
            response.EnsureSuccessStatusCode();

            var stringContent = await response.Content.ReadAsStringAsync();
            var subscriptionResponse = JsonSerializer.Deserialize<CreateSubscriptionODTO>(stringContent);

            if (subscriptionResponse == null) return null;

            return subscriptionResponse;
        }
    }
}
