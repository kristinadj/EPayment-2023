using BankPaymentService.WebApi.Models;
using Base.DTO.Input;
using Microsoft.EntityFrameworkCore;

namespace BankPaymentService.WebApi.Services
{
    public interface IMerchantService
    {
        Task<Merchant?> GetMerchantByBankRecurringTransactionId(int bankRecurringTransactionId);
        Task<Merchant?> GetMerchantByPaymentServiceMerchantId(int paymentserviceMerchantId);
        Task<bool> UpdateMerchantCredentialsAsync(UpdateMerchantCredentialsIDTO updateMerchantCredentialsIDTO);
    }

    public class MerchantService : IMerchantService
    {
        private readonly BankPaymentServiceContext _context;

        public MerchantService(BankPaymentServiceContext context)
        {
            _context = context;
        }

        public async Task<Merchant?> GetMerchantByBankRecurringTransactionId(int bankRecurringTransactionId)
        {
            return await _context.Invoices
                .Where(x => x.BankRecurringTransactionId == bankRecurringTransactionId)
                .Include(x => x.Merchant)
                .ThenInclude(x => x!.Bank)
                .Select(x => x.Merchant)
                .FirstOrDefaultAsync();
        }

        public async Task<Merchant?> GetMerchantByPaymentServiceMerchantId(int paymentserviceMerchantId)
        {
            return await _context.Merchants
                .Where(x => x.PaymentServiceMerchantId == paymentserviceMerchantId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateMerchantCredentialsAsync(UpdateMerchantCredentialsIDTO updateMerchantCredentialsIDTO)
        {
            var bank = await _context.Banks
                .Where(x => x.BankId == updateMerchantCredentialsIDTO.InstitutionId)
                .FirstOrDefaultAsync();

            if (bank == null) return false;

            var merchant = await _context.Merchants
                .Where(x => x.PaymentServiceMerchantId == updateMerchantCredentialsIDTO.PaymentServiceMerchantId)
                .FirstOrDefaultAsync();

            if (merchant == null)
            {
                merchant = new Merchant(string.Empty, updateMerchantCredentialsIDTO.Secret)
                {
                    BankMerchantId = int.Parse(updateMerchantCredentialsIDTO.Code),
                    PaymentServiceMerchantId = updateMerchantCredentialsIDTO.PaymentServiceMerchantId,
                    BankId = bank.BankId
                };

                await _context.Merchants.AddAsync(merchant);
            }
            else
            {
                merchant.BankMerchantId = int.Parse(updateMerchantCredentialsIDTO.Code);
                merchant.Secret = updateMerchantCredentialsIDTO.Secret;
                merchant.BankId = bank.BankId;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
