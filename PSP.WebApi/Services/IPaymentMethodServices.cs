using AutoMapper;
using AutoMapper.QueryableExtensions;
using Base.DTO.Shared;
using Microsoft.EntityFrameworkCore;
using PSP.WebApi.DTO.Input;
using PSP.WebApi.DTO.Output;
using PSP.WebApi.Models;

namespace PSP.WebApi.Services
{
    public interface IPaymentMethodService
    {
        Task<List<PaymentMethodODTO>> GetPaymentMethodsAsync();
        Task<PaymentMethod?> GetPaymentMethodByIdAsync(int id);
        Task<PaymentMethodODTO?> AddPaymentMethodAsync(PaymentMethodIDTO paymentMethodIDTO);
        Task<bool> UnsubscribeAsync(int paymentMethodId, int merchantId);
        Task<bool> SubscribeAsync(PspPaymentMethodSubscribeIDTO paymentMethodSubscribe);
        Task<List<PaymentMethodMerchantODTO>> GetPaymentMethodsByMerchantIdAsync(int merchantId);
        Task<List<PaymentMethodODTO>> GetActivePaymentMethodsByMerchantIdAsync(int merchantId, bool recurringPayment);
    }


    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly PspContext _context;
        private readonly IMapper _mapper;

        public PaymentMethodService(PspContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaymentMethodODTO?> AddPaymentMethodAsync(PaymentMethodIDTO paymentMethodIDTO)
        {
            var isExists = await _context.PaymentMethods
                .Where(x => x.ServiceName == paymentMethodIDTO.ServiceName && x.ServiceApiSufix == paymentMethodIDTO.ServiceApiSufix)
                .AnyAsync();

            if (isExists) return null;

            var paymentMethod = _mapper.Map<PaymentMethod>(paymentMethodIDTO);

            await _context.PaymentMethods.AddAsync(paymentMethod);
            await _context.SaveChangesAsync();
            return _mapper.Map<PaymentMethodODTO>(paymentMethod);
        }

        public async Task<PaymentMethod?> GetPaymentMethodByIdAsync(int id)
        {
            return await _context.PaymentMethods
                .Where(x => x.PaymentMethodId == id)
                .FirstOrDefaultAsync();
        }

        public Task<List<PaymentMethodODTO>> GetPaymentMethodsAsync()
        {
            return _context.PaymentMethods
                .ProjectTo<PaymentMethodODTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<bool> UnsubscribeAsync(int paymentMethodId, int merchantId)
        {
            var paymentMethodMerchant = await _context.PaymentMethodMerchants
                .Where(x => x.PaymentMethodId == paymentMethodId && x.MerchantId == merchantId)
                .FirstOrDefaultAsync();

            if (paymentMethodMerchant  == null) return false;

            paymentMethodMerchant.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SubscribeAsync(PspPaymentMethodSubscribeIDTO paymentMethodSubscribe)
        {
            var paymentMethodMerchant = await _context.PaymentMethodMerchants
                .Where(x => x.PaymentMethodId == paymentMethodSubscribe.PaymentMethodId && x.MerchantId == paymentMethodSubscribe.MerchantId)
                .FirstOrDefaultAsync();

            if (paymentMethodMerchant == null)
            {
                paymentMethodMerchant = new PaymentMethodMerchant
                {
                    MerchantId = paymentMethodSubscribe.MerchantId,
                    PaymentMethodId = paymentMethodSubscribe.PaymentMethodId,
                    IsActive = true
                };
                await _context.PaymentMethodMerchants.AddAsync(paymentMethodMerchant);
            }
            else
            {
                paymentMethodMerchant.IsActive = true;
            }
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<PaymentMethodMerchantODTO>> GetPaymentMethodsByMerchantIdAsync(int merchantId)
        {
            var paymentMethods = await _context.PaymentMethodMerchants
                .Where(x => x.MerchantId == merchantId)
                .ProjectTo<PaymentMethodMerchantODTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var paymentMethodsIds = paymentMethods.Select(x => x.PaymentMethod!.PaymentMethodId).ToList();

            var notsubscribedPaymentMethods = await _context.PaymentMethods
                .Where(x => !paymentMethodsIds.Contains(x.PaymentMethodId))
                .ProjectTo<PaymentMethodMerchantODTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            paymentMethods.AddRange(notsubscribedPaymentMethods);
            return paymentMethods;
        }

        public async Task<List<PaymentMethodODTO>> GetActivePaymentMethodsByMerchantIdAsync(int merchantId, bool recurringPayment)
        {
            if (!recurringPayment)
            {
                return await _context.PaymentMethodMerchants
                    .Where(x => x.MerchantId == merchantId && x.IsActive)
                    .ProjectTo<PaymentMethodODTO>(_mapper.ConfigurationProvider)
                    .ToListAsync();
            }
            else
            {
                return await _context.PaymentMethodMerchants
                   .Where(x => x.MerchantId == merchantId && x.IsActive && x.PaymentMethod!.SupportsAutomaticPayments)
                   .ProjectTo<PaymentMethodODTO>(_mapper.ConfigurationProvider)
                   .ToListAsync();
            }
        }
    }
}
