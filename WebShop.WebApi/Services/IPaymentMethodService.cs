using AutoMapper;
using AutoMapper.QueryableExtensions;
using Base.DTO.Shared;
using Microsoft.EntityFrameworkCore;
using WebShop.DTO.Output;
using WebShop.WebApi.Models;

namespace WebShop.WebApi.Services
{
    public interface IPaymentMethodService
    {
        Task<PaymentMethod?> GetPaymentMethodById(int id);
        Task<List<PaymentMethodODTO>> GetPaymentMethodsAsync();
        Task ImportFromPspAsync(List<PaymentMethodDTO> paymentMethods);
    }

    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly WebShopContext _context;
        private readonly IMapper _mapper;

        public PaymentMethodService(WebShopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaymentMethod?> GetPaymentMethodById(int id)
        {
            return await _context.PaymentMethods
                .Where(x => x.PaymentMethodId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<PaymentMethodODTO>> GetPaymentMethodsAsync()
        {
            return await _context.PaymentMethods
                .ProjectTo<PaymentMethodODTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task ImportFromPspAsync(List<PaymentMethodDTO> paymentMethodsIDTO)
        { 
            var paymentMethods = _mapper.Map<List<PaymentMethod>>(paymentMethodsIDTO);
            await _context.PaymentMethods.AddRangeAsync(paymentMethods);
            await _context.SaveChangesAsync();
        }
    }
}
