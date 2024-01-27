using Bank1.WebApi.AppSettings;
using Bank1.WebApi.Services;
using Base.DTO.Input;
using Microsoft.Extensions.Options;

namespace Bank1.WebApi.HostedServices
{
    public class RecurringTransactionsHostedService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public RecurringTransactionsHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var transactionService = scope.ServiceProvider.GetService<ITransactionService>();
            var accountService = scope.ServiceProvider.GetService<IAccountService>();
            var appSettings = scope.ServiceProvider.GetService<IOptions<BankSettings>>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<RecurringTransactionsHostedService>>();
            var httpClient = new HttpClient();

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
                    var recurringTransactionDefinitions = await transactionService!.GetExpiringRecurringTransactionsDefinitionsAsync();

                    foreach (var recurringTransactionDefinition in recurringTransactionDefinitions)
                    {
                        var isSuccess = false;
                        var isLocalCard = recurringTransactionDefinition.PanNumber!.StartsWith(appSettings!.Value.CardStartNumbers);
                        var recurringTransaction = await transactionService.CreateRecurringTransactionAsync(recurringTransactionDefinition, recurringTransactionDefinition.RecurringTransactions!.Select(x => x.Transaction).OrderBy(x => x.Timestamp).First()!);

                        if (!isLocalCard)
                        {
                            var payTransactionIDTO = new PayTransactionIDTO
                            {
                                CardHolderName = recurringTransactionDefinition.CardHolderName!,
                                PanNumber = recurringTransactionDefinition.PanNumber,
                                ExpiratoryDate = recurringTransactionDefinition.ExpiratoryDate!,
                                CVV = (int)recurringTransactionDefinition.CVV!,
                                TransactionId = recurringTransaction!.TransactionId
                            };

                            isSuccess = await transactionService.PccSendToPayTransctionAsync(recurringTransaction.Transaction!, payTransactionIDTO, appSettings.Value.PccBankId, appSettings.Value.PccUrl);
                        }
                        else
                        {
                            var sender = await accountService!.GetAccountByCreditCardAsync(recurringTransactionDefinition.CardHolderName!, recurringTransactionDefinition.PanNumber, recurringTransactionDefinition.ExpiratoryDate!, (int)recurringTransactionDefinition.CVV!);
                            isSuccess = await transactionService.PayTransctionAsync(recurringTransaction!.Transaction!, sender!);
                        }

                        if (isSuccess)
                        {
                            
                            try
                            {
                                var response = await httpClient.PostAsync(recurringTransactionDefinition.RecurringTransactionSuccessUrl, null, stoppingToken);

                                if (response.IsSuccessStatusCode)
                                {
                                    await transactionService.UpdateRecurringTransactionDefinitionNextPaymentDateAsync(recurringTransactionDefinition);
                                }
                            } 
                            catch (Exception ex)
                            {
                                // TODO: Rollback transaction ???
                            }
                        }
                        else
                        {
                            await httpClient.PostAsync(recurringTransactionDefinition.RecurringTransactionFailureUrl, null, stoppingToken);
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex?.Message, "Error while processing reccurent transactions");
                }

                await Task.Delay(startTime, stoppingToken);
            }
        }
    }
}
