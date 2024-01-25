using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using MudBlazor.Utilities;
using WebShop.Client.Code;
using WebShop.Client.Services;

namespace WebShop.Client.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        [Inject]
        private NavigationManager? Navigation { get; set; }

        [Inject]
        protected GlobalUserSettings GlobalSettings { get; set; }

        [Inject]
        private IApiServices ApiServices { get; set; }

        [Inject]
        private ISnackbar Snackbar { get; set; }

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

        private void OnClickBuyerInvoicesArchive(MouseEventArgs e)
        {
            Navigation!.NavigateTo("/buyerInvoicesArchive");
        }

        private void OnClickMerchantInvoicesArchive(MouseEventArgs e)
        {
            Navigation!.NavigateTo("/merchantInvoicesArchive");
        }

        private async Task OnClickCancelSubscriptionAsync()
        {
            var isSuccess = await ApiServices.CancelSubscriptionAsync(GlobalSettings.UserId!);

            if (isSuccess)
            {
                GlobalSettings.IsCanceled = true;
                GlobalSettings.InvokeChange();
            }
            else
            {
                Snackbar.Add("Unexpected exception occurred during unsubscribing", Severity.Error);
            }
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
