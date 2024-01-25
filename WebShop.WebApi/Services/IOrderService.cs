using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WebShop.DTO.Output;
using WebShop.WebApi.Models;

namespace WebShop.WebApi.Services
{
    public interface IOrderService
    {
        Task<OrderODTO?> GetOrderByIdAsync(int orderId);
        Task<OrderODTO?> GetOrderByInvoiceIdAsync(int invoiceId);
        Task<OrderODTO?> CreateOrderAsync(int shoppingCartId);
        Task<OrderODTO?> CancelOrderAsync(int orderId);
        Task<List<OrderODTO>> GetOrdersByBuyerIdAsync(string userId);
        Task<List<MerchantOrderODTO>> GetOrdersByMerchantIdAsync(string userId);
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

        public async Task<OrderODTO?> CancelOrderAsync(int orderId)
        {
            var order = await _context.Orders
                .Where(x => x.OrderId == orderId)
                .Include(x => x.OrderLogs)
                .Include(x => x.MerchantOrders!)
                .ThenInclude(x => x.OrderItems)
                .FirstOrDefaultAsync();

            if (order == null) throw new Exception($"Order {orderId} not found");

            var shoppingCart = await _context.ShoppingCarts
                .Where(x => x.UserId == order.UserId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (shoppingCart == null) throw new Exception($"ShoppingCart for user {order.UserId} not found");

            order.OrderStatus = DTO.Enums.OrderStatus.CANCELED;
            order.OrderLogs!.Add(new OrderLog
            {
                OrderStatus = DTO.Enums.OrderStatus.CANCELED,
                Timestamp = DateTime.Now
            });

            var shoppingCartItems = new List<ShoppingCartItem>();
            var orderItems = order.MerchantOrders!.SelectMany(x => x.OrderItems!).ToList();
            foreach (var orderItem in orderItems!)
            {
                var shoppingCartItem = new ShoppingCartItem
                {
                    ItemId = orderItem.ItemId,
                    Quantity = orderItem.Quantity,
                    ShoppingCartId = shoppingCart.ShoppingCartId
                };
                shoppingCartItems.Add(shoppingCartItem);
            }

            await _context.ShoppingCartItems.AddRangeAsync(shoppingCartItems);
            await _context.SaveChangesAsync();

            return await _context.Orders
                .Where(x => x.OrderId == order.OrderId)
                .ProjectTo<OrderODTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<OrderODTO?> CreateOrderAsync(int shoppingCartId)
        {
            var shoppingCart = await _context.ShoppingCarts
                .Where(x => x.ShoppingCartId == shoppingCartId)
                .Include(x => x.ShoppingCartItems!)
                .ThenInclude(x => x.Item)
                .FirstOrDefaultAsync();

            if (shoppingCart == null) throw new Exception($"ShoppingCart {shoppingCartId} not found");

            var order = _mapper.Map<Order>(shoppingCart);
            order.MerchantOrders = new List<MerchantOrder>();

            var groupedShoppingCartItems = shoppingCart.ShoppingCartItems!.GroupBy(x => x.Item!.MerchantId, x => x, (key, g) => new { MerchantId = key, Items = g.ToList() });

            foreach (var group in groupedShoppingCartItems)
            {
                var merchantOrder = new MerchantOrder
                {
                    MerchantId = group.MerchantId,
                    OrderItems = _mapper.Map<List<OrderItem>>(group.Items)
                };
                order.MerchantOrders.Add(merchantOrder);
            }

            await _context.Orders.AddAsync(order);

            _context.ShoppingCartItems.RemoveRange(shoppingCart.ShoppingCartItems!);
            await _context.SaveChangesAsync();

            return _mapper.Map<OrderODTO>(order);
        }

        public async Task<OrderODTO?> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .Where(x => x.OrderId == orderId)
                .ProjectTo<OrderODTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<OrderODTO?> GetOrderByInvoiceIdAsync(int invoiceId)
        {
            return await _context.Orders
                .Where(x => x.MerchantOrders!.Any(x => x.InvoiceId == invoiceId))
                .ProjectTo<OrderODTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<List<OrderODTO>> GetOrdersByBuyerIdAsync(string userId)
        {
            return await _context.Orders
                .Where(x => x.UserId == userId)
                .ProjectTo<OrderODTO>(_mapper.ConfigurationProvider)
                .OrderByDescending(x => x.CreatedTimestamp)
                .ToListAsync();
        }

        public async Task<List<MerchantOrderODTO>> GetOrdersByMerchantIdAsync(string userId)
        {
            return await _context.MerchantOrders
               .Where(x => x.Merchant!.UserId == userId)
               .OrderByDescending(x => x.Order!.MerchantOrders)
               .ProjectTo<MerchantOrderODTO>(_mapper.ConfigurationProvider)
               .ToListAsync();
        }
    }
}