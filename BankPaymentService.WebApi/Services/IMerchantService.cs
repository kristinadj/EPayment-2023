using BankPaymentService.WebApi.Models;
using Base.DTO.Input;
using Microsoft.EntityFrameworkCore;

namespace BankPaymentService.WebApi.Services
{
    public interface IMerchantService
    {
        Task<bool> UpdateMerchantCredentialsAsync(UpdateMerchantCredentialsIDTO updateMerchantCredentialsIDTO);
    }

    public class MerchantService : IMerchantService
    {
        private readonly BankPaymentServiceContext _context;

        public MerchantService(BankPaymentServiceContext context)
        {
            _context = context;
        }

        public async Task<bool> UpdateMerchantCredentialsAsync(UpdateMerchantCredentialsIDTO updateMerchantCredentialsIDTO)
        {
            var merchant = await _context.Merchants
                .Where(x => x.PaymentServiceMerchantId == updateMerchantCredentialsIDTO.PaymentServiceMerchantId)
                .FirstOrDefaultAsync();

            if (merchant == null) return false;

            merchant.BankMerchantId = int.Parse(updateMerchantCredentialsIDTO.Code);
            merchant.Secret = updateMerchantCredentialsIDTO.Secret;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
