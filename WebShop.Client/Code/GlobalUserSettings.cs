namespace WebShop.Client.Code
{
    public class GlobalUserSettings
    {
        public string? UserId { get; set; }
        public int ShoppingCartId { get; set; }
        public int ShoppingCartItemsCount { get; set; }

        public event Action? OnChange;

        public void AddShoppingCartItems(int count)
        {
            ShoppingCartItemsCount += count;
            OnChange?.Invoke();
        }
    }
}
