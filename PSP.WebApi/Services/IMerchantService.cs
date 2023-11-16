using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PSP.WebApi.DTO.Input;
using PSP.WebApi.DTO.Output;
using PSP.WebApi.Models;

namespace PSP.WebApi.Services
{
    public interface IMerchantService
    {
        Task<MerchantODTO?> AddMerchantAsync(MerchantIDTO merchantIDTO);
    }

    public class MerchantService : IMerchantService
    {
        private readonly PspContext _context;
        private readonly IMapper _mapper;

        public MerchantService(PspContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MerchantODTO?> AddMerchantAsync(MerchantIDTO merchantIDTO)
        {
            var isExists = await _context.Merchants
                .Where(x => x.ServiceName == merchantIDTO.ServiceName && x.MerchantExternalId == merchantIDTO.MerchantExternalId)
                .AnyAsync();

            if (isExists) return null;

            var merchant = _mapper.Map<Merchant>(merchantIDTO);
            await _context.Merchants.AddAsync(merchant);
            await _context.SaveChangesAsync();
            return _mapper.Map<MerchantODTO>(merchant);
        }
    }
}
