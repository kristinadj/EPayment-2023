using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WebShop.WebApi.DTO;
using WebShop.WebApi.Models;

namespace WebShop.WebApi.Services
{
    public interface ISubscriptionPlanService
    {
        Task<List<SubscriptionPlanDTO>> GetSubscriptionPlansAsync();
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

        public async Task<List<SubscriptionPlanDTO>> GetSubscriptionPlansAsync()
        {
            return await _context.SubscriptionPlans
                .ProjectTo<SubscriptionPlanDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
