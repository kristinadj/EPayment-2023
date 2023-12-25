using Bank2.WebApi.AppSettings;
using Bank2.WebApi.Models;
using Bank2.WebApi.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<BankSettings>(builder.Configuration.GetSection("BankSettings"));

builder.Services.AddDbContext<BankContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MainDatabase")));


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", new CorsPolicyBuilder()
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin()
        .Build());
});

#region Services

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<INbsClient, NbsClient>();

#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(c => c.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
