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
        Task<bool> UserSubscriptionPlanRenewalAsync(int userSubscriptionPlanId, TransactionStatus transactionStatus);
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

            if (!isUserValid) throw new Exception($"User {userSubscriptionPlanIDTO.UserId} is not buyer");

            var subscriptionPlan = await _context.SubscriptionPlans
                .Where(x => x.SubscriptionPlanId == userSubscriptionPlanIDTO.SubscriptionPlanId)
                .FirstOrDefaultAsync();

            if (subscriptionPlan == null) throw new Exception($"SubscriptionPlan {userSubscriptionPlanIDTO.SubscriptionPlanId} not found");

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

            if (subscriptionPlan == null) throw new Exception($"SubscriptionPlan for user {userId} not found");

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

            if (userSubscriptionPlan == null) throw new Exception($"UserSubscriptionPlan fro invoice {invoiceId} not found");

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

        public async Task<bool> UserSubscriptionPlanRenewalAsync(int userSubscriptionPlanId, TransactionStatus transactionStatus)
        {
            var userSubscriptionPlan = await _context.UserSubscriptionPlans
                .Where(x => x.UserSubscriptionPlanId == userSubscriptionPlanId && x.SubscriptionPlan!.AutomaticRenewel)
                .Include(x => x.Invoice)
                .ThenInclude(x => x!.Transaction)
                .Include(x => x.SubscriptionPlan)
                .FirstOrDefaultAsync();

            if (userSubscriptionPlan == null) throw new Exception($"UserSubscriptionPlan {userSubscriptionPlanId} not found");

            var renewedUserSubscriptionPlan = new UserSubscriptionPlan(userSubscriptionPlan.UserId)
            {
                SubscriptionPlanId = userSubscriptionPlan.SubscriptionPlanId,
                StartTimestamp = DateTime.Today,
                EndTimestamp = DateTime.Today.AddDays(userSubscriptionPlan.SubscriptionPlan!.DurationInDays),
                ExternalSubscriptionId = userSubscriptionPlan.ExternalSubscriptionId,
                Invoice = new Invoice(userSubscriptionPlan.UserId)
                {
                    MerchantId = userSubscriptionPlan.Invoice!.MerchantId,
                    TotalPrice = userSubscriptionPlan.Invoice!.TotalPrice,
                    CurrencyId = userSubscriptionPlan.Invoice!.CurrencyId,
                    InvoiceType = userSubscriptionPlan.Invoice!.InvoiceType,
                    Transaction = new Transaction
                    {
                        CreatedTimestamp = DateTime.Now,
                        TransactionStatus = transactionStatus,
                        PaymentMethodId = userSubscriptionPlan.Invoice!.Transaction!.PaymentMethodId,
                        TransactionLogs = new List<TransactionLog>()
                        {
                            new() { TransactionStatus = transactionStatus, Timestamp = DateTime.Now }
                        }
                    }
                }
            };

            await _context.UserSubscriptionPlans.AddAsync(renewedUserSubscriptionPlan);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
