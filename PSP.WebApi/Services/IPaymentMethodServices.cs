using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    }
}
