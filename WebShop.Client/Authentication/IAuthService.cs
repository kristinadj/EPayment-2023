using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using WebShop.DTO.Input;
using WebShop.DTO.Output;

namespace WebShop.Client.Authentication
{
    public interface IAuthService
    {
        Task<AuthenticationODTO?> Register(UserIDTO model);
        Task<AuthenticationODTO?> Login(AuthenticateIDTO model);
        Task Logout();
    }

    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthService(IHttpClientFactory httpClientFactory,
                           AuthenticationStateProvider authenticationStateProvider,
                           ILocalStorageService localStorage)
        {
            _httpClient = httpClientFactory.CreateClient("WebShopAPI");
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
        }

        public async Task<AuthenticationODTO?> Login(AuthenticateIDTO model)
        {
            var content = JsonSerializer.Serialize(model);
            var response = await _httpClient.PostAsync("api/Users/Authenticate", new StringContent(content, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                var authResult = await response.Content.ReadFromJsonAsync<AuthenticationODTO>();
                if (authResult == null)
                    return null;

                await _localStorage.SetItemAsync("authToken", authResult.Token);
                ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(model.Email);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Token);

                return authResult;
            }

            return null;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<AuthenticationODTO?> Register(UserIDTO model)
        {
            var content = JsonSerializer.Serialize(model);
            var response = await _httpClient.PostAsync("api/Users/Register", new StringContent(content, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                return null;

            var authResult = await response.Content.ReadFromJsonAsync<AuthenticationODTO>();
            if (authResult == null)
                return null;

            await _localStorage.SetItemAsync("authToken", authResult.Token);
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(model.Email);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Token);

            return authResult;
        }
    }
}
