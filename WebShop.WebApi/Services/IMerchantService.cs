using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebShop.WebApi.Models;

namespace WebShop.WebApi.Services
{
    public interface IMerchantService
    {
        Task<Merchant?> GetMerchantByIdAsync(int merchantId);
        Task<Merchant?> GetMerchantByUserIdAsync(string userId);

        Task<bool> UpdatePspMerchantId(int merchantId, int pspMerchantId);
    }

    public class MerchantService : IMerchantService
    {
        private readonly WebShopContext _context;
        private readonly IMapper _mapper;

        public MerchantService(WebShopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Merchant?> GetMerchantByIdAsync(int merchantId)
        {
            return await _context.Merchants
                .Where(x => x.MerchantId == merchantId)
                .Include(x => x.User)
                .FirstOrDefaultAsync();
        }

        public async Task<Merchant?> GetMerchantByUserIdAsync(string userId)
        {
            return await _context.Merchants
                .Where(x => x.UserId == userId)
                .Include(x => x.User)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdatePspMerchantId(int merchantId, int pspMerchantId)
        {
            var merchant = await _context.Merchants
                .Where(x => x.MerchantId == merchantId)
                .Include(x => x.User)
                .FirstOrDefaultAsync();

            if (merchant == null) return false;

            merchant.PspMerchantId = pspMerchantId;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
