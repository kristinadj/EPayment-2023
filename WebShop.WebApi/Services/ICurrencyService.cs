using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WebShop.DTO.Output;
using WebShop.WebApi.Models;

namespace WebShop.WebApi.Services
{
    public interface ICurrencyService
    {
        Task<List<CurrencyODTO>> GetCurrenciesAsync();
    }

    public class CurrencyService : ICurrencyService
    {
        private readonly WebShopContext _context;
        private readonly IMapper _mapper;

        public CurrencyService(WebShopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CurrencyODTO>> GetCurrenciesAsync()
        {
            return await _context.Currencies
                .ProjectTo<CurrencyODTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
