using Bank1.Client.Authentication;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;

namespace Bank1.Client.Handlers
{
    public class JwtAuthenticationHeaderHandler : DelegatingHandler
    {
        private readonly AccessTokenProvider _tokenProvider;
        private readonly NavigationManager _navManager;

        public JwtAuthenticationHeaderHandler(AccessTokenProvider accessTokenProvider, NavigationManager navigationManager)
        {
            _tokenProvider = accessTokenProvider;
            _navManager = navigationManager;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _tokenProvider.GetTokenAsync();

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
