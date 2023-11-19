using Bank1.WebApi.AppSettings;
using Bank1.WebApi.Models;
using Bank1.WebApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<UrlAppSettings>(builder.Configuration.GetSection("UrlAppSettings"));

builder.Services.AddDbContext<BankContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MainDatabase")));


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Services

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
