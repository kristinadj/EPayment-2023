using Bank2.WebApi.DTO.Input;
using Bank2.WebApi.Models;
using Base.DTO.Shared;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Bank2.WebApi.Services
{
    public interface ITransactionService
    {
        Task<Transaction?> CreateTransactionAsync(TransactionIDTO transactionIDTO);
        Task<Transaction?> GetTransactionByIdAsync(int transactionId);
        Task<bool> PayTransctionAsync(Transaction transaction, Account account);
        Task<RedirectUrlDTO?> UpdatePaymentServiceInvoiceStatusAsync(string url);
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
    }
}
