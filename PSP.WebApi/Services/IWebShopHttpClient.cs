using System.Text.Json;

namespace PSP.WebApi.Services
{
    public interface IWebShopHttpClient
    {
        Task<bool> PutAsync(string serviceName, string requestUri);
    }

    public class WebShopHttpClient : IWebShopHttpClient
    {
        private readonly HttpClient _client;

        public WebShopHttpClient(HttpClient httpClient)
        {
            _client = httpClient;
        }


        public async Task<bool> PutAsync(string serviceName, string requestUri)
        {
            var uri = $"{serviceName}/{requestUri}";
            var response = await _client.PutAsync(uri, null);

            if (!response.IsSuccessStatusCode) return false;
            return true;
        }
    }
}
