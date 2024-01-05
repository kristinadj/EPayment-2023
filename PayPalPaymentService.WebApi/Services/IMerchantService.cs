using Base.DTO.Input;
using Microsoft.EntityFrameworkCore;
using PayPalPaymentService.WebApi.Models;
using System.Runtime.CompilerServices;

namespace PayPalPaymentService.WebApi.Services
{
    public interface IMerchantService
    {
        Task<Merchant?> GetMerchantByPaymentServiceMerchantId(int paymentserviceMerchantId);
        Task<bool> UpdateMerchantCredentialsAsync(UpdateMerchantCredentialsIDTO updateMerchantCredentialsIDTO);
        Task<Merchant> UpdateProductIdAsync(Merchant merchant, string productId);
        Task<Merchant> UpdateBillingPlanIdAsync(Merchant merchant, string billngPlanId);
    }

    public class MerchantService : IMerchantService
    {
        private readonly PayPalServiceContext _context;

        public MerchantService(PayPalServiceContext context)
        {
            _context = context;
        }

        public async Task<Merchant?> GetMerchantByPaymentServiceMerchantId(int paymentserviceMerchantId)
        {
            return await _context.Merchants
                .Where(x => x.PaymentServiceMerchantId == paymentserviceMerchantId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateMerchantCredentialsAsync(UpdateMerchantCredentialsIDTO updateMerchantCredentialsIDTO)
        {
            var merchant = await _context.Merchants
                .Where(x => x.PaymentServiceMerchantId == updateMerchantCredentialsIDTO.PaymentServiceMerchantId)
                .FirstOrDefaultAsync();

            if (merchant == null) return false;

            merchant.ClientId = updateMerchantCredentialsIDTO.Code;
            merchant.Secret = updateMerchantCredentialsIDTO.Secret;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Merchant> UpdateProductIdAsync(Merchant merchant, string productId)
        {
            merchant.PayPalBillingPlanProductId = productId;

            _context.Entry(merchant).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return merchant;
        }

        public async Task<Merchant> UpdateBillingPlanIdAsync(Merchant merchant, string billngPlanId)
        {
            merchant.PayPalBillingPlanId = billngPlanId;

            _context.Entry(merchant).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return merchant;
        }
    }
}
