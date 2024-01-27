using Base.DTO.Shared;
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
            var pspApiHttpClient = scope.ServiceProvider.GetService<IPspApiHttpClient>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ImportPaymentMethodsHostedService>>();

            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.UtcNow;
                var startTime = new DateTime(now.Year, now.Month, now.Day, 2, 0, 0).AddDays(1) - DateTime.UtcNow;
                if (startTime.TotalHours > 24)
                {
                    startTime = new DateTime(now.Year, now.Month, now.Day, 2, 0, 0) - DateTime.UtcNow;
                }

                var isSuccess = false;

                do
                {
                    try
                    {
                        var result = await pspApiHttpClient!.GetAsync<List<PaymentMethodDTO>>("PaymentMethod");
                        if (result != null)
                        {
                            await paymentMethodsService!.ImportFromPspAsync(result);
                            isSuccess = true;
                        }

                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex?.Message, "Error while getting PaymentMethods from PSP");
                    }

                    await Task.Delay(10000, stoppingToken);
                } while (!isSuccess);


                await Task.Delay(startTime, stoppingToken);
            }
        }
    }
}
