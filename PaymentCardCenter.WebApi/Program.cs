using Microsoft.EntityFrameworkCore;
using PaymentCardCenter.WebApi.Models;
using PaymentCardCenter.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PccContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MainDatabase")));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Services

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
