using Base.DTO.Input;
using BitcoinPaymentService.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BitcoinPaymentService.WebApi.Services
{
    public interface IMerchantService
    {
        Task<Merchant?> GetMerchantByPaymentServiceMerchantId(int paymentserviceMerchantId);
        Task<bool> UpdateMerchantCredentialsAsync(UpdateMerchantCredentialsIDTO updateMerchantCredentialsIDTO);
    }

    public class MerchantService : IMerchantService
    {
        private readonly BitcoinServiceContext _context;

        public MerchantService(BitcoinServiceContext context)
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

            if (merchant == null)
            {
                merchant = new Merchant(updateMerchantCredentialsIDTO.Code, updateMerchantCredentialsIDTO.Secret)
                {
                    PaymentServiceMerchantId = updateMerchantCredentialsIDTO.PaymentServiceMerchantId
                };

                await _context.Merchants.AddAsync(merchant);
            }
            else
            {
                merchant.Token = updateMerchantCredentialsIDTO.Code;
                merchant.PairingCode = updateMerchantCredentialsIDTO.Secret;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
