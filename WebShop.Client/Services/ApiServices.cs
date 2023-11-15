using System.Net.Http.Json;
using WebShop.DTO;

namespace WebShop.Client.Services
{
    public class ApiServices : IApiServices
    {
        private readonly HttpClient _httpClient;

        public ApiServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ItemDTO>> GetItemsAsync()
        {
            var data = new List<ItemDTO>();

            try
            {
                var response = await _httpClient.GetAsync("api/Items");
                response.EnsureSuccessStatusCode();

                var tempData = await response.Content.ReadFromJsonAsync<List<ItemDTO>>();
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
