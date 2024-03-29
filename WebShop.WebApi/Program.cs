using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebShop.WebApi.AppSettings;
using WebShop.WebApi.HostedServices;
using WebShop.WebApi.Models;
using WebShop.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<WebShopAppSettings>(builder.Configuration.GetSection("WebShopAppSettings"));

builder.Services.AddDbContext<WebShopContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MainDatabase")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", new CorsPolicyBuilder()
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin()
        .Build());
});

builder.Services.AddIdentityCore<User>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = false;

}).AddEntityFrameworkStores<WebShopContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    });

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var apiUrl = builder.Configuration["PspAppSettings:WebApiUrl"];
builder.Services.AddHttpClient("PspAPI", x =>
{
    x.BaseAddress = new Uri(apiUrl);
});

#region Services

builder.Services.AddHostedService<ImportPaymentMethodsHostedService>();
builder.Services.AddHostedService<UpdateExpiredTransactionStatusHostedService>();

builder.Services.AddScoped<IPspApiHttpClient, PspApiHttpClient>();
builder.Services.AddScoped<ITokenCreationService, JwtService>();


builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IItemService, ItemServices>();
builder.Services.AddScoped<IMerchantService, MerchantService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPaymentMethodService, PaymentMethodService>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddScoped<ISubscriptionPlanService, SubscripionPlanService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(c => c.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
