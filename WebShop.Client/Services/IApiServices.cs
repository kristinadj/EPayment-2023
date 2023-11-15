using System.Runtime.CompilerServices;
using WebShop.DTO.Input;
using WebShop.DTO.Output;

namespace WebShop.Client.Services
{
    public interface IApiServices
    {
        Task<List<ItemODTO>> GetItemsAsync();
        Task<ShoppingCartODTO?> GetShoppingCartByUserAsync(string userId);
        Task<bool> AddItemInShoppingCartAsync(ShoppingCartItemIDTO itemDTO);
    }


}
