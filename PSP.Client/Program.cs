using MudBlazor.Services;
using PSP.Client.Components;
using PSP.Client.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

var apiUrl = builder.Configuration.GetValue<string>("ApiUrl");
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiUrl!) });

builder.Services.AddMudServices();
builder.Services.AddScoped<IApiServices, ApiServices>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
