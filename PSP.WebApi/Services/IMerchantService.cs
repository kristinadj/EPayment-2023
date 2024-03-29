﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PSP.WebApi.DTO.Input;
using PSP.WebApi.DTO.Output;
using PSP.WebApi.Models;

namespace PSP.WebApi.Services
{
    public interface IMerchantService
    {
        Task<Merchant?> GetMerchantByIdAsync(int merchantId);
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
            var merchant = await _context.Merchants
                .Where(x => x.ServiceName == merchantIDTO.ServiceName && x.MerchantExternalId.Equals(merchantIDTO.MerchantExternalId))
                .FirstOrDefaultAsync();

            if (merchant == null)
            {
                merchant = _mapper.Map<Merchant>(merchantIDTO);
                await _context.Merchants.AddAsync(merchant);
                await _context.SaveChangesAsync();
            }

            return _mapper.Map<MerchantODTO>(merchant);
        }

        public async Task<Merchant?> GetMerchantByIdAsync(int merchantId)
        {
            return await _context.Merchants
                .Where(x => x.MerchantId == merchantId)
                .Include(x => x.PaymentMethods)
                .FirstOrDefaultAsync();
        }
    }
}
