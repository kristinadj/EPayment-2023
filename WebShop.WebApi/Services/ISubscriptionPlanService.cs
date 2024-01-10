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
        Task<UserSubscriptionPlanDetailsODTO?> GetSubscriptionPlanDetailsAsync(string userId);
        Task<UserSubscriptionPlan?> GetUserSubscriptionPlanByUserIdAsync(string userId);
        Task<UserSubscriptionPlan?> AddUserSubscriptionPlanAsync(UserSubscriptionPlanIDTO userSubscriptionPlanIDTO);
        Task<bool> UpdateExternalSubscriptionIdAsync(int invoiceId, string externalSubscriptionId);
        Task CancelUserSubscriptionPlanAsync(UserSubscriptionPlan userSubscriptionPlan);
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
                SubscriptionPlan = subscriptionPlan,
                StartTimestamp = DateTime.Now,
                EndTimestamp = DateTime.Today.AddDays(subscriptionPlan.DurationInDays),
                IsCanceled = false
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

        public async Task<UserSubscriptionPlanDetailsODTO?> GetSubscriptionPlanDetailsAsync(string userId)
        {
            var subscriptionPlan = await _context.UserSubscriptionPlans
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.StartTimestamp)
                .Include(x => x.Invoice)
                .ThenInclude(x => x!.Transaction)
                .Include(x => x.SubscriptionPlan)
                .FirstOrDefaultAsync();

            if (subscriptionPlan == null) return null;

            var result = new UserSubscriptionPlanDetailsODTO
            {
                IsValid = subscriptionPlan.Invoice!.Transaction!.TransactionStatus == TransactionStatus.COMPLETED,
                ActiveUntil = subscriptionPlan.EndTimestamp,
                AutomaticRenewel = subscriptionPlan.SubscriptionPlan!.AutomaticRenewel,
                IsCanceled = subscriptionPlan.IsCanceled
            };
            return result;
        }

        public async Task<UserSubscriptionPlan?> GetUserSubscriptionPlanByUserIdAsync(string userId)
        {
            return await _context.UserSubscriptionPlans
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.StartTimestamp)
                .Include(x => x.Invoice)
                .ThenInclude(x => x!.Transaction)
                .Include(x => x.SubscriptionPlan)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateExternalSubscriptionIdAsync(int invoiceId, string externalSubscriptionId)
        {
            var userSubscriptionPlan = await _context.UserSubscriptionPlans
                .Where(x => x.InvoiceId == invoiceId && x.SubscriptionPlan!.AutomaticRenewel)
                .FirstOrDefaultAsync();

            if (userSubscriptionPlan == null) return false;

            userSubscriptionPlan.ExternalSubscriptionId = externalSubscriptionId;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task CancelUserSubscriptionPlanAsync(UserSubscriptionPlan userSubscriptionPlan)
        {
            userSubscriptionPlan.IsCanceled = true;
            _context.Entry(userSubscriptionPlan).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
