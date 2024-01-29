using Base.Services.AppSettings;
using BitcoinPaymentService.WebApi.AppSettings;
using BitcoinPaymentService.WebApi.BitcoinClients;
using BitcoinPaymentService.WebApi.Configurations;
using BitcoinPaymentService.WebApi.Models;
using BitcoinPaymentService.WebApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<BitcoinServiceContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MainDatabase")));

builder.Services.Configure<PaymentMethod>(builder.Configuration.GetSection("PaymentMethod"));
builder.Services.Configure<BitcoinSettings>(builder.Configuration.GetSection("BitcoinSettings"));

builder.Services.AddScoped<IMerchantService, MerchantService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<ICryptoCloudClient, CryptoCloudClient>();
builder.Services.AddScoped<ICoingateClient, CoingateClient>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddConsul();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseConsul();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
