using Base.Services.AppSettings;
using Microsoft.EntityFrameworkCore;
using PayPalPaymentService.WebApi.AppSettings;
using PayPalPaymentService.WebApi.Configurations;
using PayPalPaymentService.WebApi.Models;
using PayPalPaymentService.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PayPalServiceContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MainDatabase")));

builder.Services.AddControllers();

builder.Services.Configure<PaymentMethod>(builder.Configuration.GetSection("PaymentMethod"));
builder.Services.Configure<PayPalSettings>(builder.Configuration.GetSection("PayPalSettings"));

builder.Services.AddScoped<IMerchantService, MerchantService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IPayPalClientService, PayPalClientService>();

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
