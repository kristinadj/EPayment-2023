using Bank1.WebApi.DTO.Input;
using Bank1.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Bank1.WebApi.Services
{
    public interface IAccountService
    {
        Task<Account?> GetAccountByCreditCardAsync(PayTransactionIDTO payTransactionIDTO);
    }

    public class AccountService : IAccountService
    {
        private readonly BankContext _context;

        public AccountService(BankContext context)
        {
            _context = context;
        }

        public async Task<Account?> GetAccountByCreditCardAsync(PayTransactionIDTO payTransactionIDTO)
        {
            return await _context.Accounts
                .Where(x => x.Cards!.Any(x => x.CardHolderName == payTransactionIDTO.CardHolderName && x.PanNumber == payTransactionIDTO.PanNumber && x.ExpiratoryDate == payTransactionIDTO.ExpiratoryDate && x.CVV == payTransactionIDTO.CVV))
                .FirstOrDefaultAsync();
        }
    }
}
