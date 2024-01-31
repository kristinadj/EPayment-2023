using Bank.DTO.Input;
using Bank2.WebApi.Enums;
using Bank2.WebApi.Helpers;
using Bank2.WebApi.Models;
using Base.DTO.Input;
using Base.DTO.Output;
using Base.DTO.Shared;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace Bank2.WebApi.Services
{
    public interface ITransactionService
    {
        Task<Transaction?> CreateTransactionAsync(TransactionIDTO transactionIDTO);
        Task<Transaction?> CreateRecurringTransactionAsync(TransactionIDTO transactionIDTO);
        Task<RecurringTransaction?> CreateRecurringTransactionAsync(RecurringTransactionDefinition recurringTransactionDefinition, Transaction initialTransaction);
        Task<Transaction?> GetTransactionByIdAsync(int transactionId);
        Task<Transaction?> GetTransactionByBankPaymentServiceTransactionIdIdAsync(int extrenalTransactionId);
        Task<bool> PayLocalTransactionAsync(Transaction transaction, Account account);
        Task<bool> PccSendAcquirerTransactionAsync(Transaction transaction, PayTransactionIDTO payTransactionIDTO, int bankId, string pccUrl);
        Task<bool> PccSendIssuerTransactionAsync(Transaction transaction, Account issuerAccount, int bankId, string pccUrl);
        Task<PccAquirerTransactionODTO?> PccReceiveAquirerTransactionAsync(PccAquirerTransactionIDTO transactionIDTO, string cardStartNumbers);
        Task<PccIssuerTransactionODTO?> PccReceiveIsssuerTransactionAsync(PccIssuerTransactionIDTO transactionIDTO);
        Task<RedirectUrlDTO?> UpdatePaymentServiceInvoiceStatusAsync(string url);
        Task UpdateTransactionStatusAsync(Transaction transaction, TransactionStatus transactionStatus);
        Task<double?> ExchangeAsync(string fromCurrency, string toCuurency, double amount);
        Task<RecurringTransactionDefinition?> GetReccurringTransactionDefinitionByTransactionIdAsync(int transactionId);
        Task<bool> CancelRecurringTransactionAsync(int recurringTransactionDefinitionId);
        Task UpdatePaymentDataAsync(RecurringTransactionDefinition recurringTransactionDefinition, PayTransactionIDTO payTransactionIDTO);
        Task<List<RecurringTransactionDefinition>> GetExpiringRecurringTransactionsDefinitionsAsync();
        Task UpdateRecurringTransactionDefinitionNextPaymentDateAsync(RecurringTransactionDefinition recurringTransactionDefinition);
        Task UpdateAcquirerAccountIdAsync(Transaction transaction, Account acquirerAccount);
    }

    public class TransactionService : ITransactionService
    {
        private readonly BankContext _context;

        public TransactionService(BankContext context)
        {
            _context = context;
        }

        public async Task<Transaction?> CreateTransactionAsync(TransactionIDTO transactionIDTO)
        {
            var currency = await _context.Currencies
                .Where(x => x.Code == transactionIDTO.CurrencyCode)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (currency == null) return null;

            var businsessCustomers = await _context.BusinessCustomer.ToListAsync();
            var customer = await _context.BusinessCustomer
                .Where(x => x.BusinessCustomerId == transactionIDTO.SenderId && x.SecretKey == transactionIDTO.Secret)
                .Include(x => x.Customer!.Accounts)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (customer == null) return null;

            Account? account;

            if (!string.IsNullOrEmpty(transactionIDTO.AccountNumber))
            {
                account = customer.Customer!.Accounts!
                    .Where(x => x.AccountNumber == transactionIDTO.AccountNumber)
                    .FirstOrDefault();
            }
            else
            {
                var defaultAccountId = customer.DefaultAccountId;
                account = customer.Customer!.Accounts!.Where(x => x.AccountId == defaultAccountId).FirstOrDefault();
            }

            if (account == null) return null;

            var transaction = new Transaction(transactionIDTO.ExternalInvoiceId.ToString(), transactionIDTO.TransactionSuccessUrl, transactionIDTO.TransactionFailureUrl, transactionIDTO.TransactionErrorUrl)
            {
                BankPaymentServiceTransactionId = transactionIDTO.ExternalInvoiceId,
                Amount = transactionIDTO.Amount,
                CurrencyId = currency.CurrencyId,
                AquirerAccountId = account.AccountId,
                TransactionStatus = TransactionStatus.CREATED,
                Timestamp = transactionIDTO.Timestamp,
                TransactionLogs = new List<TransactionLog>
                {
                    new() { TransactionStatus = TransactionStatus.CREATED, Timestamp = DateTime.Now }
                }
            };

            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }

        public async Task<Transaction?> CreateRecurringTransactionAsync(TransactionIDTO transactionIDTO)
        {
            var currency = await _context.Currencies
                .Where(x => x.Code == transactionIDTO.CurrencyCode)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (currency == null) return null;

            var businsessCustomers = await _context.BusinessCustomer.ToListAsync();
            var customer = await _context.BusinessCustomer
                .Where(x => x.BusinessCustomerId == transactionIDTO.SenderId && x.SecretKey == transactionIDTO.Secret)
                .Include(x => x.Customer!.Accounts)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (customer == null) return null;

            var account = customer.Customer!.Accounts!
                .Where(x => x.AccountNumber == transactionIDTO.AccountNumber)
                .FirstOrDefault();
            if (account == null) return null;

            var recurringTransaction = new RecurringTransactionDefinition
            {
                Amount = transactionIDTO.Amount,
                CurrencyId = currency.CurrencyId,
                AquirerAccountId = account.AccountId,
                IsCanceled = false,
                RecurringCycleDays = 365,
                StartTimestamp = DateTime.Today,
                NextPaymentTimestamp = DateTime.Today.AddDays(365),
                RecurringTransactionSuccessUrl = transactionIDTO.RecurringTransactionSuccessUrl,
                RecurringTransactionFailureUrl = transactionIDTO.RecurringTransactionFailureUrl,
                RecurringTransactions = new List<RecurringTransaction>()
                {
                    new()
                    {
                        Transaction =  new Transaction(transactionIDTO.ExternalInvoiceId.ToString(), transactionIDTO.TransactionSuccessUrl, transactionIDTO.TransactionFailureUrl, transactionIDTO.TransactionErrorUrl)
                        {
                            Amount = transactionIDTO.Amount,
                            CurrencyId = currency.CurrencyId,
                            IssuerAccountId = account.AccountId,
                            TransactionStatus = TransactionStatus.CREATED,
                            Timestamp = transactionIDTO.Timestamp,
                            TransactionLogs = new List<TransactionLog>
                            {
                                new() { TransactionStatus = TransactionStatus.CREATED, Timestamp = DateTime.Now }
                            }
                        }
                    }
                }
            };

            await _context.RecurringTransactionDefinitions.AddAsync(recurringTransaction);
            await _context.SaveChangesAsync();

            return recurringTransaction.RecurringTransactions.Select(x => x.Transaction).FirstOrDefault();
        }

        public async Task<Transaction?> GetTransactionByIdAsync(int transactionId)
        {
            return await _context.Transactions
                .Where(x => x.TransactionId == transactionId)
                .Include(x => x.Currency)
                .Include(x => x.AquirerAccount)
                .ThenInclude(x => x!.Owner)
                .Include(x => x.TransactionLogs)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> PayLocalTransactionAsync(Transaction transaction, Account sender)
        {
            var isSuccess = true;

            double senderAmount = transaction.Amount;
            if (sender.Currency!.Code != transaction!.Currency!.Code)
            {
                senderAmount = (double)await ExchangeAsync(transaction!.Currency!.Code, sender.Currency.Code, transaction.Amount);
            }

            if (sender.Balance < senderAmount)
            {
                transaction.TransactionStatus = TransactionStatus.FAIL;
                transaction.TransactionLogs!.Add(new TransactionLog
                {
                    TransactionStatus = TransactionStatus.FAIL,
                    Timestamp = DateTime.Now
                });

                return false;
            }
            else
            {
                var receiver = await _context.Accounts
                    .Where(x => x.AccountId == transaction.AquirerAccountId)
                    .Include(x => x.Currency)
                    .FirstOrDefaultAsync();

                if (receiver == null) throw new Exception($"Account {transaction.AquirerAccountId} not found");

                double receiverAmount = transaction.Amount;
                if (receiver!.Currency!.Code != transaction!.Currency!.Code)
                {
                    receiverAmount = (double)await ExchangeAsync(transaction!.Currency!.Code, receiver.Currency.Code, transaction.Amount);
                }

                receiver!.Balance += receiverAmount;
                sender.Balance -= senderAmount;

                transaction.TransactionStatus = TransactionStatus.COMPLETED;
                transaction.TransactionLogs!.Add(new TransactionLog
                {
                    TransactionStatus = TransactionStatus.COMPLETED,
                    Timestamp = DateTime.Now
                });

                isSuccess = true;
            }

            await _context.SaveChangesAsync();
            return isSuccess;
        }

        public async Task<RedirectUrlDTO?> UpdatePaymentServiceInvoiceStatusAsync(string url)
        {
            try
            {
                using var client = new HttpClient();
                var response = await client.PutAsync(url, null);

                if (!response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"{url} responded with status code {response.StatusCode} and message {responseContent}");
                }

                var content = await response.Content.ReadAsStringAsync();
                if (content == null)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"No response {url}");
                }

                return JsonSerializer.Deserialize<RedirectUrlDTO>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task UpdateTransactionStatusAsync(Transaction transaction, TransactionStatus transactionStatus)
        {
            transaction.TransactionStatus = transactionStatus;
            transaction.TransactionLogs!.Add(new TransactionLog { TransactionStatus = transactionStatus, Timestamp = DateTime.Now });
            await _context.SaveChangesAsync();
        }

        public async Task<bool> PccSendAcquirerTransactionAsync(Transaction transaction, PayTransactionIDTO payTransactionIDTO, int bankId, string pccUrl)
        {
            var currency = await _context.Currencies
                .Where(x => x.CurrencyId == transaction.CurrencyId)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (currency == null) throw new Exception($"Currency {transaction.CurrencyId} not found");

            var pccTransactionIDTO = new PccAquirerTransactionIDTO(currency!.Code, transaction.Description)
            {
                AquirerBankId = bankId,
                AquirerTransctionId = transaction.TransactionId,
                AquirerTimestamp = transaction.Timestamp,
                Amount = transaction.Amount,
                CurrencyCode = currency.Code,
                Description = transaction.Description,
                ExternalInvoiceId = transaction.BankPaymentServiceTransactionId,
                TransactionSuccessUrl = transaction.TransactionSuccessUrl,
                TransactionFailureUrl = transaction.TransactionFailureUrl,
                TransactionErrorUrl = transaction.TransactionErrorUrl,
                PayTransaction = payTransactionIDTO,
            };

            using var httpClient = new HttpClient();
            var content = new StringContent(JsonSerializer.Serialize(pccTransactionIDTO), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync($"{pccUrl}/Transaction/ReceiveAcquirerTransaction", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var transactionODTO = JsonSerializer.Deserialize<PccAquirerTransactionODTO>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (transactionODTO != null)
                {
                    var aquirerTransaction = new AcqurierTransaction
                    {
                        TransactionId = transaction.TransactionId,
                        IssuerTransactionId = transactionODTO.IssuerTransactionId,
                        IssuerTimestamp = transactionODTO.IssuerTimestamp
                    };

                    await _context.AcqurierTransactions.AddAsync(aquirerTransaction);

                    if (transactionODTO.IsSuccess)
                    {
                        var account = await _context.Accounts
                           .Where(x => x.AccountId == transaction.AquirerAccountId)
                           .Include(x => x.Currency)
                           .FirstOrDefaultAsync();

                        var senderAmount = transaction.Amount;
                        if (account!.Currency!.Code != transaction!.Currency!.Code)
                        {
                            senderAmount = (double)await ExchangeAsync(transaction!.Currency!.Code, account.Currency.Code, transaction.Amount);
                        }

                        account!.Balance += senderAmount;

                        transaction.TransactionStatus = Enums.TransactionStatus.COMPLETED;
                        transaction.TransactionLogs!.Add(new TransactionLog { TransactionStatus = TransactionStatus.COMPLETED, Timestamp = DateTime.Now });
                    }

                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"PCC responded with status code {response.StatusCode} and message {responseContent}");
            }

            return true;
        }

        public async Task<bool> PccSendIssuerTransactionAsync(Transaction transaction, Account issuerAccount, int bankId, string pccUrl)
        {
            var currency = await _context.Currencies
                .Where(x => x.CurrencyId == transaction.CurrencyId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (currency == null) throw new Exception($"Currency {transaction.CurrencyId} not found");

            var senderAmount = transaction.Amount;
            if (issuerAccount!.Currency!.Code != transaction!.Currency!.Code)
            {
                senderAmount = (double)await ExchangeAsync(transaction!.Currency!.Code, issuerAccount.Currency.Code, transaction.Amount);
            }

            if (issuerAccount.Balance < senderAmount)
            {
                return false;
            }

            var pccTransactionIDTO = new PccIssuerTransactionIDTO(currency!.Code, transaction.Description, transaction.AquirerAccount!.AccountNumber)
            {
                IssuerBankId = bankId,
                IssuerTransctionId = transaction.TransactionId,
                IssuerTimestamp = transaction.Timestamp,
                Amount = transaction.Amount,
                CurrencyCode = currency.Code,
                Description = transaction.Description,
                ExternalInvoiceId = transaction.BankPaymentServiceTransactionId,
                TransactionSuccessUrl = transaction.TransactionSuccessUrl,
                TransactionFailureUrl = transaction.TransactionFailureUrl,
                TransactionErrorUrl = transaction.TransactionErrorUrl
            };

            using var httpClient = new HttpClient();
            var content = new StringContent(JsonSerializer.Serialize(pccTransactionIDTO), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync($"{pccUrl}/Transaction/ReceiveIssuerTransaction", content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var transactionODTO = JsonSerializer.Deserialize<PccIssuerTransactionODTO>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                var issuerTransaction = new IssuerTransaction
                {
                    TransactionId = transaction.TransactionId,
                    AquirerTransactionId = transactionODTO!.AcquirerTransactionId,
                    AquirerTimestamp = transactionODTO.AcquirerTimestamp
                };

                await _context.IssuerTransactions.AddAsync(issuerTransaction);

                if (transactionODTO.IsSuccess)
                {
                    issuerAccount.Balance -= senderAmount;
                    _context.Entry(issuerAccount).State = EntityState.Modified;

                    transaction.TransactionStatus = Enums.TransactionStatus.COMPLETED;
                    transaction.TransactionLogs!.Add(new TransactionLog { TransactionStatus = TransactionStatus.COMPLETED, Timestamp = DateTime.Now });
                }

                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<PccAquirerTransactionODTO?> PccReceiveAquirerTransactionAsync(PccAquirerTransactionIDTO transactionIDTO, string cardStartNumbers)
        {
            var currency = await _context.Currencies
                .Where(x => x.Code == transactionIDTO.CurrencyCode)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (currency == null) throw new Exception($"Currency {transactionIDTO.CurrencyCode} not found");

            var isLocalCard = transactionIDTO.PayTransaction!.PanNumber.StartsWith(cardStartNumbers);
            if (!isLocalCard) throw new Exception($"Card is not local");

            var hashedPanNumber = Converter.HashPanNumber(transactionIDTO.PayTransaction.PanNumber);
            var issuerAccount = await _context.Accounts
                .Where(x => x.Cards!.Any(x => x.CardHolderName == transactionIDTO.PayTransaction.CardHolderName && x.PanNumber == hashedPanNumber && x.ExpiratoryDate == transactionIDTO.PayTransaction.ExpiratoryDate && x.CVV == transactionIDTO.PayTransaction.CVV))
                .FirstOrDefaultAsync();

            if (issuerAccount == null) throw new Exception($"Invalid credit card data");

            var transaction = new Transaction(transactionIDTO.ExternalInvoiceId.ToString(), transactionIDTO.TransactionSuccessUrl, transactionIDTO.TransactionFailureUrl, transactionIDTO.TransactionErrorUrl)
            {
                BankPaymentServiceTransactionId = transactionIDTO.ExternalInvoiceId,
                Amount = transactionIDTO.Amount,
                CurrencyId = currency.CurrencyId,
                IssuerAccountId = issuerAccount.AccountId,
                TransactionStatus = TransactionStatus.CREATED,
                Timestamp = DateTime.Now,
                TransactionLogs = new List<TransactionLog>
                {
                    new() { TransactionStatus = TransactionStatus.CREATED, Timestamp = DateTime.Now }
                }
            };

            var issuerTransaction = new IssuerTransaction
            {
                Transaction = transaction,
                AquirerTransactionId = transactionIDTO.AquirerTransctionId,
                AquirerTimestamp = transactionIDTO.AquirerTimestamp
            };

            var senderAmount = transaction.Amount;
            if (issuerAccount.Currency!.Code != transaction!.Currency!.Code)
            {
                senderAmount = (double)await ExchangeAsync(transaction!.Currency!.Code, issuerAccount.Currency.Code, transaction.Amount);
            }

            if (issuerAccount.Balance < senderAmount)
            {
                transaction.TransactionStatus = TransactionStatus.FAIL;
                transaction.TransactionLogs.Add(new TransactionLog { TransactionStatus = TransactionStatus.FAIL, Timestamp = DateTime.Now });
            }
            else
            {
                issuerAccount.Balance -= senderAmount;
                transaction.TransactionStatus = TransactionStatus.COMPLETED;
                transaction.TransactionLogs.Add(new TransactionLog { TransactionStatus = TransactionStatus.COMPLETED, Timestamp = DateTime.Now });
            }

            await _context.IssuerTransactions.AddAsync(issuerTransaction);
            await _context.SaveChangesAsync();

            var transactionODTO = new PccAquirerTransactionODTO(issuerAccount.AccountNumber, transactionIDTO.CurrencyCode)
            {
                AquirerTransctionId = transactionIDTO.AquirerTransctionId,
                AquirerTimestamp = transactionIDTO.AquirerTimestamp,
                IssuerTransactionId = transaction.TransactionId,
                IssuerTimestamp = transaction.Timestamp,
                Amount = transactionIDTO.Amount,
                IsSuccess = transaction.TransactionStatus == TransactionStatus.COMPLETED
            };
            return transactionODTO;
        }

        public async Task<PccIssuerTransactionODTO?> PccReceiveIsssuerTransactionAsync(PccIssuerTransactionIDTO transactionIDTO)
        {
            var currency = await _context.Currencies
                .Where(x => x.Code == transactionIDTO.CurrencyCode)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (currency == null) throw new Exception($"Currency {transactionIDTO.CurrencyCode} not found");

            Account? acquirerAccount;

            if (transactionIDTO.UseHyphens)
            {
                acquirerAccount = await _context.Accounts
                    .Where(x => x.AccountNumber == transactionIDTO.AcquirerAccountNumber)
                    .Include(x => x.Currency)
                    .FirstOrDefaultAsync();
            }
            else
            {
                acquirerAccount = await _context.Accounts
                    .Where(x => x.AccountNumber.Replace("-", string.Empty) == transactionIDTO.AcquirerAccountNumber)
                    .Include(x => x.Currency)
                    .FirstOrDefaultAsync();
            }

            if (acquirerAccount == null) throw new Exception($"Invalid account {transactionIDTO.AcquirerAccountNumber}");

            var transaction = new Transaction(transactionIDTO.ExternalInvoiceId.ToString(), transactionIDTO.TransactionSuccessUrl, transactionIDTO.TransactionFailureUrl, transactionIDTO.TransactionErrorUrl)
            {
                BankPaymentServiceTransactionId = transactionIDTO.ExternalInvoiceId,
                Amount = transactionIDTO.Amount,
                CurrencyId = currency.CurrencyId,
                IssuerAccountId = acquirerAccount.AccountId,
                TransactionStatus = TransactionStatus.CREATED,
                Timestamp = DateTime.Now,
                TransactionLogs = new List<TransactionLog>
                {
                    new() { TransactionStatus = TransactionStatus.CREATED, Timestamp = DateTime.Now }
                }
            };

            var senderAmount = transaction.Amount;
            if (acquirerAccount.Currency!.Code != transaction!.Currency!.Code)
            {
                senderAmount = (double)await ExchangeAsync(transaction!.Currency!.Code, acquirerAccount.Currency!.Code, transaction.Amount);
            }
            acquirerAccount.Balance += senderAmount;

            var acquireTransaction = new AcqurierTransaction
            {
                Transaction = transaction,
                IssuerTransactionId = transactionIDTO.IssuerTransctionId,
                IssuerTimestamp = transactionIDTO.IssuerTimestamp
            };

            await _context.AcqurierTransactions.AddAsync(acquireTransaction);
            await _context.SaveChangesAsync();

            var transactionODTO = new PccIssuerTransactionODTO(transactionIDTO.CurrencyCode)
            {
                IssuerTransctionId = transactionIDTO.IssuerTransctionId,
                IssuerTimestamp = transactionIDTO.IssuerTimestamp,
                AcquirerTransactionId = acquireTransaction.TransactionId,
                AcquirerTimestamp = acquireTransaction.Transaction.Timestamp,
                Amount = transactionIDTO.Amount,
                IsSuccess = transaction.TransactionStatus == TransactionStatus.COMPLETED
            };
            return transactionODTO;
        }

        public async Task<double?> ExchangeAsync(string fromCurrency, string toCuurency, double amount)
        {
            var exchangeRate = await _context.ExchangeRates
                .Where(x => x.FromCurrency!.Code == fromCurrency && x.ToCurrency!.Code == toCuurency)
                .FirstOrDefaultAsync();

            if (exchangeRate == null) throw new Exception($"Exhange rate from {fromCurrency} to {toCuurency} not found");

            return amount * exchangeRate.Rate;
        }

        public async Task<RecurringTransactionDefinition?> GetReccurringTransactionDefinitionByTransactionIdAsync(int transactionId)
        {
            return await _context.RecurringTransactions
                .Where(x => x.TransactionId == transactionId)
                .Select(x => x.RecurringTransactionDefinition)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> CancelRecurringTransactionAsync(int recurringTransactionDefinitionId)
        {
            var recurringTransactionDefinition = await _context.RecurringTransactionDefinitions
                .Where(x => x.RecurringTransactionDefinitionId == recurringTransactionDefinitionId)
                .FirstOrDefaultAsync();

            if (recurringTransactionDefinition == null) throw new Exception($"RecurringTransactionDefinition {recurringTransactionDefinitionId} not found");

            recurringTransactionDefinition.IsCanceled = true;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task UpdatePaymentDataAsync(RecurringTransactionDefinition recurringTransactionDefinition, PayTransactionIDTO payTransactionIDTO)
        {
            recurringTransactionDefinition.CardHolderName = payTransactionIDTO.CardHolderName;
            recurringTransactionDefinition.PanNumber = payTransactionIDTO.PanNumber;
            recurringTransactionDefinition.ExpiratoryDate = payTransactionIDTO.ExpiratoryDate;
            recurringTransactionDefinition.CVV = payTransactionIDTO.CVV;

            _context.Entry(recurringTransactionDefinition).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<List<RecurringTransactionDefinition>> GetExpiringRecurringTransactionsDefinitionsAsync()
        {
            return await _context.RecurringTransactionDefinitions
                .Where(x => x.NextPaymentTimestamp == DateTime.Today && !x.IsCanceled)
                .Include(x => x.RecurringTransactions!)
                .ThenInclude(x => x.Transaction)
                .ToListAsync();
        }

        public async Task UpdateRecurringTransactionDefinitionNextPaymentDateAsync(RecurringTransactionDefinition recurringTransactionDefinition)
        {
            recurringTransactionDefinition.NextPaymentTimestamp = DateTime.Today.AddDays(recurringTransactionDefinition.RecurringCycleDays);
            _context.Entry(recurringTransactionDefinition).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<RecurringTransaction?> CreateRecurringTransactionAsync(RecurringTransactionDefinition recurringTransactionDefinition, Transaction initialTransaction)
        {
            var recurringTransaction = new RecurringTransaction
            {
                Transaction = new Transaction(string.Empty, string.Empty, string.Empty, string.Empty)
                {
                    Amount = recurringTransactionDefinition.Amount,
                    CurrencyId = recurringTransactionDefinition.CurrencyId,
                    AquirerAccountId = initialTransaction.AquirerAccountId,
                    TransactionStatus = TransactionStatus.CREATED,
                    Timestamp = DateTime.Now,
                    TransactionLogs = new List<TransactionLog>
                    {
                        new() { TransactionStatus = TransactionStatus.CREATED, Timestamp = DateTime.Now }
                    }
                },
                RecurringTransactionDefinitionId = recurringTransactionDefinition.RecurringTransactionDefinitionId
            };

            await _context.RecurringTransactions.AddAsync(recurringTransaction);
            await _context.SaveChangesAsync();

            return recurringTransaction;
        }

        public async Task UpdateAcquirerAccountIdAsync(Transaction transaction, Account acquirerAccount)
        {
            transaction.AquirerAccountId = acquirerAccount.AccountId;
            _context.Entry<Transaction>(transaction).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<Transaction?> GetTransactionByBankPaymentServiceTransactionIdIdAsync(int extrenalTransactionId)
        {
            return await _context.Transactions
                .Where(x => x.BankPaymentServiceTransactionId == extrenalTransactionId)
                .Include(x => x.Currency)
                .Include(x => x.AquirerAccount)
                .ThenInclude(x => x!.Owner)
                .Include(x => x.TransactionLogs)
                .FirstOrDefaultAsync();
        }
    }
}
