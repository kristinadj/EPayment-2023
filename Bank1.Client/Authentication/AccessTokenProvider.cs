using Blazored.LocalStorage;

namespace Bank1.Client.Authentication
{
    public class AccessTokenProvider
    {
        private readonly ILocalStorageService _localStorage;

        public AccessTokenProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task<string> GetTokenAsync()
        {
            return await _localStorage.GetItemAsync<string>("authToken");
        }
    }
}
