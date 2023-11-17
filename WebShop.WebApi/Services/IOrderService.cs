using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WebShop.DTO.Output;
using WebShop.WebApi.Models;

namespace WebShop.WebApi.Services
{
    public interface IOrderService
    {
        Task<OrderODTO?> GetByIdAsync(int orderId);
        Task<OrderODTO?> CreateOrderAsync(int shoppingCartId);
    }

    public class OrderService : IOrderService
    {
        private readonly WebShopContext _context;
        private readonly IMapper _mapper;

        public OrderService(WebShopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OrderODTO?> CreateOrderAsync(int shoppingCartId)
        {
            var shoppingCart = await _context.ShoppingCarts
                .Where(x => x.ShoppingCartId == shoppingCartId)
                .Include(x => x.ShoppingCartItems!)
                .ThenInclude(x => x.Item)
                .FirstOrDefaultAsync();

            if (shoppingCart == null) return null;

            var order = _mapper.Map<Order>(shoppingCart);
            order.MerchantId = shoppingCart.ShoppingCartItems!.First().Item!.MerchantId;
            await _context.Orders.AddAsync(order);

            _context.ShoppingCartItems.RemoveRange(shoppingCart.ShoppingCartItems!);
            await _context.SaveChangesAsync();

            return _mapper.Map<OrderODTO>(order);
        }

        public async Task<OrderODTO?> GetByIdAsync(int orderId)
        {
            return await _context.Orders
                .Where(x => x.OrderId == orderId)
                .ProjectTo<OrderODTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }
    }
}
