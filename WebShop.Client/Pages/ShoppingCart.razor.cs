using Microsoft.AspNetCore.Components;
using WebShop.Client.Code;
using WebShop.Client.Services;
using WebShop.DTO.Output;

namespace WebShop.Client.Pages
{
    public partial class ShoppingCart
    {
        [Inject]
        private IApiServices ApiServices { get; set; }

        [Inject]
        protected GlobalUserSettings GlobalSettings { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private bool isLoading = false;

        private ShoppingCartODTO? shoppingCart;

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            shoppingCart = await ApiServices.GetShoppingCartByUserAsync(GlobalSettings.UserId!);

            GlobalSettings.UpdateShoppingCartItems(-GlobalSettings.ShoppingCartItemsCount);
            GlobalSettings.UpdateShoppingCartItems(shoppingCart!.ShoppingCartItems!.Select(x => x.Quantity).Sum());

            isLoading = false;
        }

        private async Task OnClickCheckoutAsync()
        {
            var order = await ApiServices.CreateOrderAsync(GlobalSettings.ShoppingCartId!);
            if (order != null)
            {
                GlobalSettings.UpdateShoppingCartItems(-GlobalSettings.ShoppingCartItemsCount);
                NavigationManager.NavigateTo($"/order/{order.OrderId}");
            }
        }
    }
}
