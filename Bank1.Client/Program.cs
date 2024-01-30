using Bank1.Client;
using Bank1.Client.Authentication;
using Bank1.Client.Handlers;
using Bank1.Client.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiUrl = builder.Configuration.GetValue<string>("ApiUrl");
builder.Services.AddHttpClient("BankAPI", x =>
{
    x.BaseAddress = new Uri(apiUrl);
})
.AddHttpMessageHandler<JwtAuthenticationHeaderHandler>()
.AddHttpMessageHandler<CustomHttpMessageHandler>();

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomCenter;
});

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();

builder.Services.AddSingleton<ISnackbar, SnackbarService>();

builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
builder.Services.AddScoped<AccessTokenProvider>();
builder.Services.AddScoped<CustomHttpMessageHandler>();
builder.Services.AddScoped<JwtAuthenticationHeaderHandler>();

builder.Services.AddScoped<IApiServices, ApiServices>();
builder.Services.AddScoped<IAuthService, AuthService>();

await builder.Build().RunAsync();
