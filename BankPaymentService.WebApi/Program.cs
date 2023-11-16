using BankPaymentService.WebApi.AppSettings;
using BankPaymentService.WebApi.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<CardPaymentMethod>(builder.Configuration.GetSection("CardPaymentMethod"));
builder.Services.Configure<QrCodePaymentMethod>(builder.Configuration.GetSection("QrCodePaymentMethod"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddConsul();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
