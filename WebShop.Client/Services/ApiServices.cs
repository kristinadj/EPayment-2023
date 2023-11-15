using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using WebShop.DTO.Input;
using WebShop.DTO.Output;

namespace WebShop.Client.Services
{
    public class ApiServices : IApiServices
    {
        private readonly HttpClient _httpClient;

        public ApiServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ItemODTO>> GetItemsAsync()
        {
            var data = new List<ItemODTO>();

            try
            {
                var response = await _httpClient.GetAsync("api/Items");
                response.EnsureSuccessStatusCode();

                var tempData = await response.Content.ReadFromJsonAsync<List<ItemODTO>>();
                if (tempData != null) { data = tempData; }
            }
            catch (Exception ex)
            {
                // TODO:
            }

            return data;
        }

        public async Task<ShoppingCartODTO?> GetShoppingCartByUserAsync(string userId)
        {
            ShoppingCartODTO? data = null;

            try
            {
                var response = await _httpClient.GetAsync($"api/ShoppingCart/ByUser/{userId}");
                response.EnsureSuccessStatusCode();

                var tempData = await response.Content.ReadFromJsonAsync<ShoppingCartODTO>();
                if (tempData != null) { data = tempData; }
            }
            catch (Exception ex)
            {
                // TODO:
            }

            return data;
        }

        public async Task<bool> AddItemInShoppingCartAsync(ShoppingCartItemIDTO itemDTO)
        {
            var isSuccess = false;

            try
            {
                var content = new StringContent(JsonSerializer.Serialize(itemDTO), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"api/ShoppingCartItem", content);
                response.EnsureSuccessStatusCode();

                isSuccess = true;
            }
            catch (Exception ex)
            {
                // TODO:
            }

            return isSuccess;
        }
    }
}
