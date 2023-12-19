using PSP.Client.DTO.Output;
using System.Net.Http.Json;

namespace PSP.Client.Services
{
    public interface IApiServices
    {
        Task<List<PaymentMethodODTO>> GetPaymentMethodsAsync(int merchantId);
        Task<InvoiceODTO?> GetInvoiceByIdAsync(int invoiceId);
        Task<RedirectUrlDTO?> UpdatePaymentMethodAsync(int invoiceId, int paymentMethodId);
    }

    public class ApiServices : IApiServices
    {
        private readonly HttpClient _httpClient;

        public ApiServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PaymentMethodODTO>> GetPaymentMethodsAsync(int merchantId)
        {
            var data = new List<PaymentMethodODTO>();

            try
            {
                var response = await _httpClient.GetAsync($"api/PaymentMethod/Active/ByMerchantId/{merchantId}");
                response.EnsureSuccessStatusCode();

                var str = await response.Content.ReadAsStringAsync();
                var tempData = await response.Content.ReadFromJsonAsync<List<PaymentMethodODTO>>();
                if (tempData != null) { data = tempData; }
            }
            catch (Exception ex)
            {
                // TODO:
            }

            return data;
        }

        public async Task<InvoiceODTO?> GetInvoiceByIdAsync(int invoiceId)
        {
            InvoiceODTO? data = null;

            try
            {
                var response = await _httpClient.GetAsync($"api/Invoice/{invoiceId}");
                response.EnsureSuccessStatusCode();

                var str = await response.Content.ReadAsStringAsync();
                var tempData = await response.Content.ReadFromJsonAsync<InvoiceODTO>();
                if (tempData != null) { data = tempData; }
            }
            catch (Exception ex)
            {
                // TODO:
            }

            return data;
        }

        public async Task<RedirectUrlDTO?> UpdatePaymentMethodAsync(int invoiceId, int paymentMethodId)
        {
            RedirectUrlDTO? data = null;

            try
            {
                var response = await _httpClient.PutAsync($"api/Invoice/PaymentMethod/{invoiceId};{paymentMethodId}", null);
                response.EnsureSuccessStatusCode();

                var str = await response.Content.ReadAsStringAsync();
                var tempData = await response.Content.ReadFromJsonAsync<RedirectUrlDTO>();
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
