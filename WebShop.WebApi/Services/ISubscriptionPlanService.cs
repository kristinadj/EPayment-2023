﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WebShop.DTO.Output;
using WebShop.WebApi.Models;

namespace WebShop.WebApi.Services
{
    public interface ISubscriptionPlanService
    {
        Task<List<SubscriptionPlanODTO>> GetSubscriptionPlansAsync();
    }

    public class SubscripionPlanService : ISubscriptionPlanService
    {
        private readonly WebShopContext _context;
        private readonly IMapper _mapper;

        public SubscripionPlanService(WebShopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<SubscriptionPlanODTO>> GetSubscriptionPlansAsync()
        {
            return await _context.SubscriptionPlans
                .ProjectTo<SubscriptionPlanODTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
