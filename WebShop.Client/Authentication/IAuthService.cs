using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using WebShop.DTO.Input;
using WebShop.DTO.Output;
using System.Net.Http.Json;
using System.IO.Pipelines;

namespace WebShop.Client.Authentication
{
    public interface IAuthService
    {
        Task<AuthenticationODTO> Login(AuthenticateIDTO model);
        Task Logout();
        Task<AuthenticationODTO?> Register(UserIDTO model);
    }

    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthService(HttpClient httpClient,
                           AuthenticationStateProvider authenticationStateProvider,
                           ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
        }

        public async Task<AuthenticationODTO?> Login(AuthenticateIDTO model)
        {
            var content = JsonSerializer.Serialize(model);
            var response = await _httpClient.PostAsync("api/Users/Authenticate", new StringContent(content, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                return null;

            var loginResult = await response.Content.ReadFromJsonAsync<AuthenticationODTO>();
            if (loginResult == null)
                return null;

            await _localStorage.SetItemAsync("authToken", loginResult.Token);
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(model.Email);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult.Token);

            return loginResult;
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

            return authResult;
        }
    }
}
