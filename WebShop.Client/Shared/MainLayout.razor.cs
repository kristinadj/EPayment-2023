using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MudBlazor;
using MudBlazor.Utilities;
using System.Security.Claims;
using WebShop.Client.Code;
using WebShop.Client.Services;

namespace WebShop.Client.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        [Inject]
        private NavigationManager? Navigation { get; set; }

        [Inject]
        private IApiServices ApiServices { get; set; }

        [Inject]
        protected GlobalUserSettings GlobalSettings { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        private MudTheme? _currentTheme;

        protected override async Task OnInitializedAsync()
        {
            _currentTheme = new MudTheme()
            {
                Typography = new Typography()
                {
                    Default = new Default()
                    {
                        FontFamily = new[] { "Helvetica", "Arial", "sans-serif" }
                    }
                },
                Palette = new PaletteLight
                {
                    Primary = new MudColor("#37474F")
                }
            };

            var user = (await AuthenticationStateTask).User;
            if (user != null && user.Identity!.IsAuthenticated)
            {
                var userId = user.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
                if (userId != null)
                {
                    GlobalSettings.UserId = userId.Value;

                    var shoppingCart = await ApiServices.GetShoppingCartByUserAsync(GlobalSettings.UserId);
                    if (shoppingCart != null)
                    {
                        GlobalSettings.ShoppingCartId = shoppingCart.ShoppingCartId;
                        GlobalSettings.ShoppingCartItemsCount = shoppingCart.ShoppingCartItems!.Select(x => x.Quantity).Sum();

                        GlobalSettings.OnChange += RefreshShoppingCartItem;
                    }
                }
            }
            else
            {
                Navigation!.NavigateTo("/login");
            }
        }

        private void OnClickHome(MouseEventArgs e)
        {
            Navigation!.NavigateTo("/");
        }

        private void OnClickShoppingCart(MouseEventArgs e)
        {
            Navigation!.NavigateTo("/shoppingcart");
        }

        private void OnClickLogout(MouseEventArgs e)
        {
            Navigation!.NavigateTo("/logout");
        }

        private void RefreshShoppingCartItem()
        {
            StateHasChanged();
        }

        public void Dispose()
        {
            GlobalSettings.OnChange -= RefreshShoppingCartItem;
            GC.SuppressFinalize(this);
        }
    }
}
