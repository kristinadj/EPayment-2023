using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WebShop.DTO.Input;
using WebShop.DTO.Output;
using WebShop.WebApi.Models;

namespace WebShop.WebApi.Services
{
    public interface IShoppingCartService
    {
        Task<ShoppingCartODTO?> GetShoppingCartByUserAsync(string userId);
        Task CreateShoppingCartAsync(string userId);
        Task<bool> AddItemInShoppingCartAsync(ShoppingCartItemIDTO itemDTO);
        Task<bool> DeleteItemInShoppingCartAsync(int shoppingCartItemId);
    }

    public class ShoppingCartService : IShoppingCartService
    {
        private readonly WebShopContext _context;
        private readonly IMapper _mapper;

        public ShoppingCartService(WebShopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AddItemInShoppingCartAsync(ShoppingCartItemIDTO itemDTO)
        {
            var shoppingCart = await _context.ShoppingCarts
                .Where(x => x.ShoppingCartId == itemDTO.ShoppingCartId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (shoppingCart == null) return false;

            var item = await _context.Items
                .Where(x => x.ItemId == itemDTO.ItemId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (item == null) return false;

            var shopiingCartItem = new ShoppingCartItem
            {
                ShoppingCartId = itemDTO.ShoppingCartId,
                ItemId = itemDTO.ItemId,
                Quantity = itemDTO.Quantity
            };
            await _context.ShoppingCartItems.AddAsync(shopiingCartItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task CreateShoppingCartAsync(string userId)
        {
            var shoppingCart = new ShoppingCart(userId);
            await _context.ShoppingCarts.AddAsync(shoppingCart);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteItemInShoppingCartAsync(int shoppingCartItemId)
        {
            var shoppingCartItem = await _context.ShoppingCartItems
                .Where(x => x.ShoppingCartItemId == shoppingCartItemId)
                .FirstOrDefaultAsync();

            if (shoppingCartItem == null) return false;

            _context.ShoppingCartItems.Remove(shoppingCartItem);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ShoppingCartODTO?> GetShoppingCartByUserAsync(string userId)
        {
            return await _context.ShoppingCarts
                .Where(x => x.UserId == userId)
                .ProjectTo<ShoppingCartODTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }
    }
}
