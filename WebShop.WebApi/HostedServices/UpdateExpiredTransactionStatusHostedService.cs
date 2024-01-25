
using WebShop.WebApi.Services;

namespace WebShop.WebApi.HostedServices
{
    public class UpdateExpiredTransactionStatusHostedService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public UpdateExpiredTransactionStatusHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var transactionService = scope.ServiceProvider.GetService<ITransactionService>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<UpdateExpiredTransactionStatusHostedService>>();

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var toDateTime = DateTime.Now.AddMinutes(-7);
                    var transactions = await transactionService!.GetTransactionsCreatedBeforeGivenDateTimeAsync(toDateTime);
                    foreach (var transaction in transactions)
                    {
                        await transactionService.UpdateTransactionStatusAsync(transaction.TransactionId, DTO.Enums.TransactionStatus.EXPIRED);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex?.Message, "Error while updating expired Transaction statuses");
                }

                await Task.Delay(TimeSpan.FromMinutes(7), stoppingToken);
            }
        }
    }
}
