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

        private ShoppingCartODTO? shoppingCart;

        protected override async Task OnInitializedAsync()
        {
            shoppingCart = await ApiServices.GetShoppingCartByUserAsync(GlobalSettings.UserId!);
        }
    }
}
