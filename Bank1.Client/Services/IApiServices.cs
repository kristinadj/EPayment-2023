using Bank1.Client.DTO.Input;
using Base.DTO.Shared;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;

namespace Bank1.Client.Services
{
    public interface IApiServices
    {
        Task<RedirectUrlDTO?> PayTransactionAsync(PayTransactionIDTO payTransactionIDTO);
    }

    public class ApiServices : IApiServices
    {
        private readonly HttpClient _httpClient;

        public ApiServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<RedirectUrlDTO?> PayTransactionAsync(PayTransactionIDTO payTransactionIDTO)
        {
            RedirectUrlDTO? data = null;

            try
            {
                var content = new StringContent(JsonSerializer.Serialize(payTransactionIDTO), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"/Transaction", content);
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
    }
}
