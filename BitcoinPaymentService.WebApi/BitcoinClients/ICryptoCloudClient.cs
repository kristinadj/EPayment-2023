using BitcoinPaymentService.WebApi.DTO.CryptoCloud.Input;
using BitcoinPaymentService.WebApi.DTO.CryptoCloud.Output;
using System.Net.Http.Headers;
using System.Text.Json;

namespace BitcoinPaymentService.WebApi.BitcoinClients
{
    public interface ICryptoCloudClient
    {
        Task<CreateInvoiceODTO?> CreateInvoiceAsync(string apiKey, string shopId, int invoiceId, decimal amount, string currencyCode, string successUrl, string cancelUrl);
    }

    public class CryptoCloudClient : ICryptoCloudClient
    {
        private const string BASE_URL = "https://api.cryptocloud.plus";

        public async Task<CreateInvoiceODTO?> CreateInvoiceAsync(string apiKey, string shopId, int invoiceId, decimal amount, string currencyCode, string successUrl, string cancelUrl)
        {
            var invoiceIDTO = new InvoiceIDTO
            {
                ShopId = shopId,
                CurrencyCode = currencyCode,
                Amount = amount,
                OrderId = invoiceId.ToString()
            };

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var response = await httpClient.PostAsJsonAsync($"{BASE_URL}/v2/invoice/create", invoiceIDTO);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var invoiceODTO = JsonSerializer.Deserialize<CreateInvoiceODTO>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return invoiceODTO;
            }

            return null;
        }
    }
}
