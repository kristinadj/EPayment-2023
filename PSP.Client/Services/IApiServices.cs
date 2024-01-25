using PSP.Client.DTO.Output;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace PSP.Client.Services
{
    public interface IApiServices
    {
        Task<List<PaymentMethodODTO>> GetPaymentMethodsAsync(int merchantId, bool recurringPayment);
        Task<InvoiceODTO?> GetInvoiceByIdAsync(int invoiceId);
        Task<RedirectUrlDTO?> UpdatePaymentMethodAsync(int invoiceId, int paymentMethodId);
    }

    public class ApiServices : IApiServices
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public ApiServices(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("PspAPI");
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<List<PaymentMethodODTO>> GetPaymentMethodsAsync(int merchantId, bool recurringPayment)
        {
            var data = new List<PaymentMethodODTO>();

            var response = await _httpClient.GetAsync($"api/PaymentMethod/Active/ByMerchantId/{merchantId};{recurringPayment}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    var tempData = JsonSerializer.Deserialize<List<PaymentMethodODTO>>(content, _jsonSerializerOptions);
                    if (tempData != null) { data = tempData; }
                }
            }

            return data;
        }

        public async Task<InvoiceODTO?> GetInvoiceByIdAsync(int invoiceId)
        {
            InvoiceODTO? data = null;

            var response = await _httpClient.GetAsync($"api/Invoice/{invoiceId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    var tempData = JsonSerializer.Deserialize<InvoiceODTO> (content, _jsonSerializerOptions);
                    if (tempData != null) { data = tempData; }
                }
            }

            return data;
        }

        public async Task<RedirectUrlDTO?> UpdatePaymentMethodAsync(int invoiceId, int paymentMethodId)
        {
            RedirectUrlDTO? data = null;

            var response = await _httpClient.PutAsync($"api/Invoice/PaymentMethod/{invoiceId};{paymentMethodId}", null);
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
    }
}
