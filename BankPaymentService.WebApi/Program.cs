using BankPaymentService.WebApi.AppSettings;
using BankPaymentService.WebApi.Configurations;
using BankPaymentService.WebApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BankPaymentServiceContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MainDatabase")));


builder.Services.AddControllers();

builder.Services.Configure<CardPaymentMethod>(builder.Configuration.GetSection("CardPaymentMethod"));
builder.Services.Configure<QrCodePaymentMethod>(builder.Configuration.GetSection("QrCodePaymentMethod"));

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
