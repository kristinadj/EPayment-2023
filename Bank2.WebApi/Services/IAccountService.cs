using Bank2.WebApi.Helpers;
using Bank2.WebApi.Models;
using Base.DTO.Input;
using Microsoft.EntityFrameworkCore;

namespace Bank2.WebApi.Services
{
    public interface IAccountService
    {
        Task<Account?> GetAccountByCreditCardAsync(string cardHolderName, string panNumber, string expiratoryDate, int cvv);
        Task<Account?> GetAccountByCustomerIdAsync(string payerId);
        Task<Account?> GetAcountByAccountNumberAsync(string accountNumber, bool useHyphens);
    }

    public class AccountService : IAccountService
    {
        private readonly BankContext _context;

        public AccountService(BankContext context)
        {
            _context = context;
        }

        public async Task<Account?> GetAccountByCreditCardAsync(string cardHolderName, string panNumber, string expiratoryDate, int cvv)
        {
            return await _context.Accounts!
                .Where(x => x.Cards!.Any(x => x.CardHolderName == cardHolderName && x.PanNumber == panNumber && x.ExpiratoryDate == expiratoryDate && x.CVV == cvv))
                .Include(x => x.Currency)
                .FirstOrDefaultAsync();
        }

        public async Task<Account?> GetAccountByCustomerIdAsync(string payerId)
        {
            return await _context.Accounts!
                .Where(x => x.Owner!.Id == payerId && x.Balance > 0)
                .Include(x => x.Currency)
                .FirstOrDefaultAsync();
        }

        public async Task<Account?> GetAcountByAccountNumberAsync(string accountNumber, bool useHyphens)
        {
            if (useHyphens)
            {
                return await _context.Accounts
                    .Where(x => x.AccountNumber == accountNumber)
                    .Include(x => x.Currency)
                    .FirstOrDefaultAsync();
            }
            else
            {
                return await _context.Accounts
                    .Where(x => x.AccountNumber.Replace("-", string.Empty) == accountNumber)
                    .Include(x => x.Currency)
                    .FirstOrDefaultAsync();
            }
        }
    }
}
