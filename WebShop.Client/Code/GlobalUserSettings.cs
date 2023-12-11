using WebShop.DTO.Enums;

namespace WebShop.Client.Code
{
    public class GlobalUserSettings
    {
        public string? UserId { get; set; }
        public int ShoppingCartId { get; set; }
        public int ShoppingCartItemsCount { get; set; }
        public Role Role { get; set; }

        public event Action? OnChange;

        public void UpdateShoppingCartItems(int count)
        {
            ShoppingCartItemsCount += count;
            OnChange?.Invoke();
        }
    }
}
