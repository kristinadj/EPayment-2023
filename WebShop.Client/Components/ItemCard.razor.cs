using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebShop.Client.Code;
using WebShop.Client.Services;
using WebShop.DTO.Input;
using WebShop.DTO.Output;

namespace WebShop.Client.Components
{
    public partial class ItemCard
    {
        [Parameter]
        public ItemODTO? Item { get; set; }

        [Inject]
        protected GlobalUserSettings GlobalSettings { get; set; }

        [Inject]
        private IApiServices ApiServices { get; set; }

        [Inject]
        private ISnackbar Snackbar { get; set; }

        private int quantity = 1;

        private async Task OnAddToCartAsync()
        {
            GlobalSettings.UpdateShoppingCartItems(quantity);
            var itemIDTO = new ShoppingCartItemIDTO
            {
                ShoppingCartId = GlobalSettings.ShoppingCartId,
                ItemId = Item!.ItemId,
                Quantity = quantity,
            };

            var isSuccess = await ApiServices.AddItemInShoppingCartAsync(itemIDTO);
            if (isSuccess)
            {
                Snackbar.Add("Successfully added to Shopping Cart", Severity.Success);
            }
            else
            {
                Snackbar.Add("Some error occureed while adding to Shopping Cart", Severity.Warning);
            }

            StateHasChanged();
        }
    }
}
