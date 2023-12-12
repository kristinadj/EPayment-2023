using Base.DTO.Shared;
using Base.Services.Clients;
using Microsoft.Extensions.Options;
using System.Runtime;
using WebShop.WebApi.AppSettings;
using WebShop.WebApi.Services;

namespace WebShop.WebApi.HostedServices
{
    public class ImportPaymentMethodsHostedService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public ImportPaymentMethodsHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var paymentMethodsService = scope.ServiceProvider.GetService<IPaymentMethodService>();
            var consulHttpClient = scope.ServiceProvider.GetService<IConsulHttpClient>();
            var pspAppSettings = scope.ServiceProvider.GetService<IOptions<PspAppSettings>>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ImportPaymentMethodsHostedService>>();

            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.UtcNow;
                var startTime = new DateTime(now.Year, now.Month, now.Day, 2, 0, 0).AddDays(1) - DateTime.UtcNow;
                if (startTime.TotalHours > 24)
                {
                    startTime = new DateTime(now.Year, now.Month, now.Day, 2, 0, 0) - DateTime.UtcNow;
                }

                try
                {
                    var result = await consulHttpClient!.GetAsync<List<PaymentMethodDTO>>(pspAppSettings!.Value.ServiceName, "/api/PaymentMethod");
                    if (result != null) 
                    {
                        await paymentMethodsService!.ImportFromPspAsync(result);
                    }
                    
                }
                catch (Exception ex)
                {
                    logger.LogError(ex?.Message, "Error while cleaning up PoiCompanies, Locations, Locationmatrix");
                }

                await Task.Delay(startTime, stoppingToken);
            }
        }
    }
}
