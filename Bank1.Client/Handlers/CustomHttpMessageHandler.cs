using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net;

namespace Bank1.Client.Handlers
{
    public class CustomHttpMessageHandler : DelegatingHandler
    {
        private readonly NavigationManager _navManager;
        private readonly ISnackbar _snackbar;

        public CustomHttpMessageHandler(NavigationManager navManager, ISnackbar snackbar)
        {
            _navManager = navManager;
            _snackbar = snackbar;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var statusCode = response.StatusCode;
                var message = await response.Content.ReadAsStringAsync(cancellationToken);

                switch (statusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        _snackbar.Add("User is not authorized", Severity.Warning);
                        _navManager.NavigateTo("/login");
                        break;
                    case HttpStatusCode.InternalServerError:
                        _snackbar.Add(message, Severity.Error);
                        break;
                    default:
                        _snackbar.Add(message, Severity.Warning);
                        break;
                }
            }

            return response;
        }
    }
}
