using Bank1.WebApi.DTO.Input;
using Bank1.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Bank1.WebApi.Services
{
    public interface ITransactionService
    {
        Task<Transaction?> CreateTransactionAsync(TransactionIDTO transactionIDTO);
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
    }
}
