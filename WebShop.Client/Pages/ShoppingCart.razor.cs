using Microsoft.AspNetCore.Components;
using MudBlazor;
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

        [Inject]
        private ISnackbar Snackbar { get; set; }

        private bool isLoading = false;

        private ShoppingCartODTO? shoppingCart;

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;

            if (GlobalSettings.IsSubscriptionPlanValid == null)
            {
                var isSubscriptionPlanValid = await ApiServices.IsSubscriptionPlanValidAsync(GlobalSettings.UserId!);
                GlobalSettings.IsSubscriptionPlanValid = isSubscriptionPlanValid;
            }

            if (!(bool)GlobalSettings.IsSubscriptionPlanValid)
            {
                NavigationManager!.NavigateTo("/plan");
            }
            else
            {
                shoppingCart = await ApiServices.GetShoppingCartByUserAsync(GlobalSettings.UserId!);

                GlobalSettings.UpdateShoppingCartItems(-GlobalSettings.ShoppingCartItemsCount);
                GlobalSettings.UpdateShoppingCartItems(shoppingCart!.ShoppingCartItems!.Select(x => x.Quantity).Sum());
                isLoading = false;
            }
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

        private async Task OnClickDeleteAsync(int shoppingCartItemId, int quantity)
        {
            var isSuccess = await ApiServices.DeleteItemInShoppingCartAsync(shoppingCartItemId);
            if (isSuccess)
            {
                Snackbar.Add("Succesfully removed", Severity.Success);
                GlobalSettings.UpdateShoppingCartItems(-quantity);
                isLoading = true;
                shoppingCart = await ApiServices.GetShoppingCartByUserAsync(GlobalSettings.UserId!);
                isLoading = false;
                StateHasChanged();
            }
            else
            {
                Snackbar.Add("Error while removing", Severity.Error);
            }
        }
    }
}
