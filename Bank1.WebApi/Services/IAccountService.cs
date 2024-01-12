using Bank1.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Bank1.WebApi.Services
{
    public interface IAccountService
    {
        Task<Account?> GetAccountByCreditCardAsync(string cardHolderName, string panNumber, string expiratoryDate, int cvv);
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
            return await _context.Accounts
                .Where(x => x.Cards!.Any(x => x.CardHolderName == cardHolderName && x.PanNumber == panNumber && x.ExpiratoryDate == expiratoryDate && x.CVV == cvv))
                .FirstOrDefaultAsync();
        }
    }
}
