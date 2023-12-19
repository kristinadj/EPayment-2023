using Base.DTO.Shared;
using WebShop.DTO.Enums;
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
        Task<InvoiceODTO?> GetInvoiceByIdAsync(int invoiceId);
        Task<OrderODTO?> GeOrderByInvoiceIdAsync(int invoiceId);
        Task<bool> UpdateTransactionStatusAsync(int transactionid, TransactionStatus transactionStatus);
        Task<List<SubscriptionPlanODTO>> GetSubscriptionPlansAsync();
        Task<List<PaymentMethodMerchantODTO>> GetPaymentMethodsByUserIdAsync(string userId);
        Task<bool> SubscribeToPaymentMethodAsync(PaymentMethodSubscribeIDTO paymentMethodSubscribeIDTO);
        Task<bool> UnsubscribeFromPaymentMethodAsync(int paymentMethodId, string userId);
        Task<bool> IsMerchantRegisteredOnPspAsync(string userId);
        Task<bool> RegisterMerchantOnPspAsync(string userId);
        Task<bool> DeleteItemInShoppingCartAsync(int shoppingCartItemId);
        Task<bool> IsSubscriptionPlanValidAsync(string userId);
        Task<RedirectUrlDTO> ChooseSubscriptionPlanAsync(UserSubscriptionPlanIDTO userSubscriptionPlanIDTO);
    }
}
