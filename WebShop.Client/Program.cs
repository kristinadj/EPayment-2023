using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using WebShop.Client;
using WebShop.Client.Authentication;
using WebShop.Client.Code;
using WebShop.Client.Handlers;
using WebShop.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


var apiUrl = builder.Configuration.GetValue<string>("ApiUrl");

builder.Services.AddHttpClient("WebShopAPI", x =>
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

builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
builder.Services.AddScoped<AccessTokenProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IApiServices, ApiServices>();

builder.Services.AddSingleton<ISnackbar, SnackbarService>();
builder.Services.AddScoped<CustomHttpMessageHandler>();
builder.Services.AddScoped<JwtAuthenticationHeaderHandler>();


builder.Services.AddSingleton<GlobalUserSettings>();

await builder.Build().RunAsync();
