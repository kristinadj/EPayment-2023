using Bank2.WebApi.DTO.Input;
using Bank2.WebApi.Enums;
using Bank2.WebApi.Models;
using Base.DTO.Input;
using Base.DTO.Output;
using Base.DTO.Shared;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json;

namespace Bank2.WebApi.Services
{
    public interface ITransactionService
    {
        Task<Transaction?> CreateTransactionAsync(TransactionIDTO transactionIDTO);
        Task<Transaction?> GetTransactionByIdAsync(int transactionId);
        Task<bool> PayTransctionAsync(Transaction transaction, Account account);
        Task<bool> PccSendToPayTransctionAsync(Transaction transaction, PayTransactionIDTO payTransactionIDTO, int bankId, string pccUrl);
        Task<RedirectUrlDTO?> UpdatePaymentServiceInvoiceStatusAsync(string url);
        Task<PccTransactionODTO?> PccReceiveToPayTransactionAsync(PccTransactionIDTO transactionIDTO, string cardStartNumbers);
        Task UpdateTransactionStatusAsync(Transaction transaction, TransactionStatus transactionStatus);

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

            var customer = await _context.BusinessCustomer
                .Where(x => x.BusinessCustomerId == transactionIDTO.SenderId && x.Password == transactionIDTO.Secret)
                .Include(x => x.Customer!.Accounts)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (customer == null) return null;

            var account = customer.Customer!.Accounts!
                .Where(x => x.AccountNumber == transactionIDTO.AccountNumber)
                .FirstOrDefault();
            if (account == null) return null;

            // TODO: Currency conversion
            var transaction = new Transaction(transactionIDTO.ExternalInvoiceId.ToString(), transactionIDTO.TransactionSuccessUrl, transactionIDTO.TransactionFailureUrl, transactionIDTO.TransactionErrorUrl)
            {
                Amount = transactionIDTO.Amount,
                CurrencyId = currency.CurrencyId,
                ReceiverAccountId = account.AccountId,
                TransactionStatus = Enums.TransactionStatus.CREATED,
                Timestamp = transactionIDTO.Timestamp,
                TransactionLogs = new List<TransactionLog>
                {
                    new TransactionLog { TransactionStatus = Enums.TransactionStatus.CREATED, Timestamp = DateTime.Now }
                }
            };

            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }

        public async Task<Transaction?> GetTransactionByIdAsync(int transactionId)
        {
            return await _context.Transactions
                .Where(x => x.TransactionId == transactionId)
                .Include(x => x.TransactionLogs)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> PayTransctionAsync(Transaction transaction, Account sender)
        {
            // TODO: Currency conversion

            var isSuccess = true;

            if (sender.Balance < transaction.Amount)
            {
                transaction.TransactionStatus = Enums.TransactionStatus.FAIL;
                transaction.TransactionLogs!.Add(new TransactionLog
                {
                    TransactionStatus = Enums.TransactionStatus.FAIL,
                    Timestamp = DateTime.Now
                });

                isSuccess = false;
            }
            else
            {
                var receiver = await _context.Accounts
                    .Where(x => x.OwnerId == transaction.ReceiverAccountId)
                    .FirstOrDefaultAsync();

                receiver!.Balance += transaction.Amount;
                sender.Balance -= transaction.Amount;

                transaction.TransactionStatus = Enums.TransactionStatus.COMPLETED;
                transaction.TransactionLogs!.Add(new TransactionLog
                {
                    TransactionStatus = Enums.TransactionStatus.COMPLETED,
                    Timestamp = DateTime.Now
                });

                isSuccess = true;
            }

            await _context.SaveChangesAsync();
            return isSuccess;
        }

        public async Task<PccTransactionODTO?> PccReceiveToPayTransactionAsync(PccTransactionIDTO transactionIDTO, string cardStartNumbers)
        {
            var currency = await _context.Currencies
                .Where(x => x.Code == transactionIDTO.CurrencyCode)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (currency == null) return null;

            var isLocalCard = transactionIDTO.PayTransaction!.PanNumber.StartsWith(cardStartNumbers);
            if (!isLocalCard) return null;

            var issuerAccount = await _context.Accounts
                .Where(x => x.Cards!.Any(x => x.CardHolderName == transactionIDTO.PayTransaction.CardHolderName && x.PanNumber == transactionIDTO.PayTransaction.PanNumber && x.ExpiratoryDate == transactionIDTO.PayTransaction.ExpiratoryDate && x.CVV == transactionIDTO.PayTransaction.CVV))
                .FirstOrDefaultAsync();

            if (issuerAccount == null) return null;

            // TODO: Currency conversion
            var transaction = new IssuerTransaction(transactionIDTO.Description)
            {
                Amount = transactionIDTO.Amount,
                CurrencyId = currency.CurrencyId,
                IssuerAccountId = issuerAccount.AccountId,
                AquirerTransactionId = transactionIDTO.AquirerTransctionId,
                AquirerTimestamp = transactionIDTO.AquirerTimestamp,
                Timestamp = DateTime.Now
            };

            if (issuerAccount.Balance < transactionIDTO.Amount)
            {
                transaction.TransactionStatus = Enums.TransactionStatus.FAIL;
            }
            else
            {
                issuerAccount.Balance -= transaction.Amount;
                transaction.TransactionStatus = Enums.TransactionStatus.COMPLETED;
            }

            await _context.IssuerTransactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            var transactionODTO = new PccTransactionODTO(issuerAccount.AccountNumber, transactionIDTO.CurrencyCode)
            {
                AquirerTransctionId = transactionIDTO.AquirerTransctionId,
                AquirerTimestamp = transactionIDTO.AquirerTimestamp,
                IssuerTransactionId = transaction.TransactionId,
                IssuerTimestamp = transaction.Timestamp,
                Amount = transactionIDTO.Amount,
                IsSuccess = transaction.TransactionStatus == Enums.TransactionStatus.COMPLETED
            };
            return transactionODTO;
        }

        public async Task<RedirectUrlDTO?> UpdatePaymentServiceInvoiceStatusAsync(string url)
        {
            try
            {
                using var client = new HttpClient();
                var response = await client.PutAsync(url, null);

                if (!response.IsSuccessStatusCode) return null;

                var content = await response.Content.ReadAsStringAsync();
                if (content == null) return default;

                return JsonSerializer.Deserialize<RedirectUrlDTO>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task<bool> PccSendToPayTransctionAsync(Transaction transaction, PayTransactionIDTO payTransactionIDTO, int bankId, string pccUrl)
        {
            var pccTransactionIDTO = new PccTransactionIDTO(transaction.Currency!.Code, transaction.Description)
            {
                AquirerBankId = bankId,
                AquirerTransctionId = transaction.TransactionId,
                AquirerTimestamp = transaction.Timestamp,
                Amount = transaction.Amount,
                PayTransaction = payTransactionIDTO,
            };

            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsJsonAsync($"{pccUrl}/Transaction", pccTransactionIDTO);
            if (response.IsSuccessStatusCode)
            {
                var transactionODTO = await response.Content.ReadFromJsonAsync<PccTransactionODTO?>();
                if (transactionODTO != null)
                {
                    transaction.IssuerTransactionId = transactionODTO.IssuerTransactionId;
                    transaction.IssuerTimestamp = transactionODTO.IssuerTimestamp;

                    if (transactionODTO.IsSuccess)
                    {
                        var account = await _context.Accounts
                            .Where(x => x.AccountId == transaction.ReceiverAccountId)
                            .FirstOrDefaultAsync();
                        account!.Balance += transaction.Amount;

                        transaction.TransactionStatus = Enums.TransactionStatus.COMPLETED;
                        transaction.TransactionLogs!.Add(new TransactionLog { TransactionStatus = Enums.TransactionStatus.COMPLETED, Timestamp = DateTime.Now });
                    }

                    await _context.SaveChangesAsync();
                    return true;
                }
            }

            return false;
        }

        public async Task UpdateTransactionStatusAsync(Transaction transaction, TransactionStatus transactionStatus)
        {
            transaction.TransactionStatus = transactionStatus;
            transaction.TransactionLogs!.Add(new TransactionLog { TransactionStatus = transactionStatus, Timestamp = DateTime.Now });
            await _context.SaveChangesAsync();
        }
    }
}
