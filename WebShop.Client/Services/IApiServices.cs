using Base.DTO.Shared;
using WebShop.DTO.Input;
using WebShop.DTO.Output;

namespace WebShop.Client.Services
{
    public interface IApiServices
    {
        Task<List<ItemODTO>> GetItemsAsync();
        Task<ShoppingCartODTO?> GetShoppingCartByUserAsync(string userId);
        Task<bool> AddItemInShoppingCartAsync(ShoppingCartItemIDTO itemDTO);
        Task<OrderODTO?> CreateOrderAsync(int shoppingCartId);
        Task<RedirectUrlDTO?> CreateInvoiceAsync(int orderId, int paymentMethodId);
        Task<List<PaymentMethodODTO>> GetPaymentMethodsAsync();
        Task<OrderODTO?> CancelOrderAsync(int orderId);
        Task<OrderODTO?> GetOrderByIdAsync(int orderId);
    }
}
