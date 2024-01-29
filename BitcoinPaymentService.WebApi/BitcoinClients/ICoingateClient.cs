using BitcoinPaymentService.WebApi.DTO.Coingate.Output;
using BitcoinPaymentService.WebApi.DTO.CryptoCloud.Output;
using System.Net.Http.Headers;
using System.Text.Json;

namespace BitcoinPaymentService.WebApi.BitcoinClients
{
    public interface ICoingateClient
    {
        Task<CreateOrderODTO?> CreateInvoiceAsync(string apiKey, string shopId, int invoiceId, decimal amount, string currencyCode, string successUrl, string cancelUrl);
    }

    public class CoingateClient : ICoingateClient
    {
        private const string BASE_URL = "https://api-sandbox.coingate.com/v2";

        public async Task<CreateOrderODTO?> CreateInvoiceAsync(string apiKey, string shopId, int invoiceId, decimal amount, string currencyCode, string successUrl, string cancelUrl)
        {
            apiKey = "Bs2rfjbkUqaBUWG_Lb2dFpWAymFbPrD_mkvoLJdz";

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var parameters = new Dictionary<string, string>
            {
                { "order_id", invoiceId.ToString() },
                { "price_amount", amount.ToString("N2") },
                { "price_currency", currencyCode },
                { "receive_currency", "BTC" },
                { "success_url", successUrl },
                { "cancel_url", cancelUrl }
            };

            var requestBody = new FormUrlEncodedContent(parameters);
            var response = await httpClient.PostAsync($"{BASE_URL}/orders", requestBody);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var invoiceODTO = JsonSerializer.Deserialize<CreateOrderODTO>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return invoiceODTO;
            }

            return null;
        }
    }
}
