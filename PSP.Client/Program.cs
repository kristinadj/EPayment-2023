using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using PSP.Client;
using PSP.Client.Handlers;
using PSP.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiUrl = builder.Configuration.GetValue<string>("ApiUrl");

builder.Services.AddHttpClient("PspAPI", x =>
{
    x.BaseAddress = new Uri(apiUrl);
})
.AddHttpMessageHandler<CustomHttpMessageHandler>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiUrl!) });

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomCenter;
});

builder.Services.AddSingleton<ISnackbar, SnackbarService>();
builder.Services.AddScoped<CustomHttpMessageHandler>();
builder.Services.AddScoped<IApiServices, ApiServices>();

await builder.Build().RunAsync();
