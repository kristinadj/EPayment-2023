using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WebShop.DTO.Enums;
using WebShop.DTO.Input;
using WebShop.DTO.Output;
using WebShop.WebApi.Models;

namespace WebShop.WebApi.Services
{
    public interface ISubscriptionPlanService
    {
        Task<List<SubscriptionPlanODTO>> GetSubscriptionPlansAsync();
        Task<bool> IsSubscriptionPlanValidAsync(string userId);
        Task<SubscriptionPlanDetailsODTO?> GetSubscriptionPlanDetailsAsync(string userId);
        Task<UserSubscriptionPlan?> AddUserSubscriptionPlanAsync(UserSubscriptionPlanIDTO userSubscriptionPlanIDTO);
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

        public async Task<UserSubscriptionPlan?> AddUserSubscriptionPlanAsync(UserSubscriptionPlanIDTO userSubscriptionPlanIDTO)
        {
            var isUserValid = await _context.Users
                .Where(x => x.Id == userSubscriptionPlanIDTO.UserId && x.Role == Role.BUYER)
                .AnyAsync();

            var subscriptionPlan = await _context.SubscriptionPlans
                .Where(x => x.SubscriptionPlanId == userSubscriptionPlanIDTO.SubscriptionPlanId)
                .FirstOrDefaultAsync();

            if (!isUserValid || subscriptionPlan == null) return null;

            var userSubscriptionPlan = new UserSubscriptionPlan(userSubscriptionPlanIDTO.UserId)
            {
                SubscriptionPlanId = userSubscriptionPlanIDTO.SubscriptionPlanId,
                StartTimestamp = DateTime.Now,
                EndTimestamp = DateTime.Today.AddDays(subscriptionPlan.DurationInDays)
            };

            await _context.UserSubscriptionPlans.AddAsync(userSubscriptionPlan);
            await _context.SaveChangesAsync();
            return userSubscriptionPlan;
        }

        public async Task<List<SubscriptionPlanODTO>> GetSubscriptionPlansAsync()
        {
            return await _context.SubscriptionPlans
                .ProjectTo<SubscriptionPlanODTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<bool> IsSubscriptionPlanValidAsync(string userId)
        {
            return await _context.UserSubscriptionPlans
                .Where(x => x.UserId == userId && DateTime.Now >= x.StartTimestamp && DateTime.Now < x.EndTimestamp && x.Invoice != null && x.Invoice.Transaction!.TransactionStatus == TransactionStatus.COMPLETED)
                .AnyAsync();
        }

        public async Task<SubscriptionPlanDetailsODTO?> GetSubscriptionPlanDetailsAsync(string userId)
        {
            var subscriptionPlan = await _context.UserSubscriptionPlans
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.StartTimestamp)
                .Include(x => x.Invoice)
                .ThenInclude(x => x!.Transaction)
                .Include(x => x.SubscriptionPlan)
                .FirstOrDefaultAsync();

            if (subscriptionPlan == null) return null;

            var result = new SubscriptionPlanDetailsODTO
            {
                IsValid = subscriptionPlan.Invoice!.Transaction!.TransactionStatus == TransactionStatus.COMPLETED,
                ActiveUntil = subscriptionPlan.EndTimestamp,
                AutomaticRenewel = subscriptionPlan.SubscriptionPlan!.AutomaticRenewel
            };
            return result;
        }
    }
}
