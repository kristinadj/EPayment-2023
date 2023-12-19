using PSP.Client.DTO.Output;

namespace PSP.Client.Services
{
    public interface IApiServices
    {
        Task<List<PaymentMethodODTO>> GetPaymentMethodsAsync(int merchantId);
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

                var tempData = await response.Content.ReadFromJsonAsync<List<PaymentMethodODTO>>();
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
