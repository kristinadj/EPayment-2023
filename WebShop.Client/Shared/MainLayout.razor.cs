using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using MudBlazor.Utilities;
using WebShop.Client.Code;

namespace WebShop.Client.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        [Inject]
        private NavigationManager? Navigation { get; set; }

        [Inject]
        protected GlobalUserSettings GlobalSettings { get; set; }

        private MudTheme? _currentTheme;

        protected override void OnInitialized()
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

            GlobalSettings.OnChange += RefreshAppBar;
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

        private void RefreshAppBar()
        {
            StateHasChanged();
        }

        public void Dispose()
        {
            GlobalSettings.OnChange -= RefreshAppBar;
            GC.SuppressFinalize(this);
        }
    }
}
