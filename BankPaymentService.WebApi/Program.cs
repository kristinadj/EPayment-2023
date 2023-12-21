using BankPaymentService.WebApi.AppSettings;
using BankPaymentService.WebApi.Configurations;
using BankPaymentService.WebApi.Models;
using BankPaymentService.WebApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BankPaymentServiceContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MainDatabase")));


builder.Services.AddControllers();

builder.Services.Configure<BankPaymentServiceUrl>(builder.Configuration.GetSection("BankPaymentServiceUrl"));
builder.Services.Configure<CardPaymentMethod>(builder.Configuration.GetSection("CardPaymentMethod"));
builder.Services.Configure<QrCodePaymentMethod>(builder.Configuration.GetSection("QrCodePaymentMethod"));

builder.Services.AddScoped<IMerchantService, MerchantService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddConsul();

#region Services

builder.Services.AddScoped<IBankService, BankService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();

#endregion

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
