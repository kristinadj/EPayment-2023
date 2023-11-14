﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WebShop.DTO;
using WebShop.WebApi.Models;

namespace WebShop.WebApi.Services
{
    public interface ICurrencyService
    {
        Task<List<CurrencyDTO>> GetCurrenciesAsync();
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

        public async Task<List<CurrencyDTO>> GetCurrenciesAsync()
        {
            return await _context.Currencies
                .ProjectTo<CurrencyDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
