using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using MudBlazor.Utilities;

namespace WebShop.Client.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        [Inject]
        private NavigationManager? Navigation { get; set; }

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
        }

        private void OnClickHome(MouseEventArgs e)
        {
            Navigation?.NavigateTo("/");
        }

        private void OnClickLogout(MouseEventArgs e)
        {
            Navigation?.NavigateTo("/logout");
        }
    }
}
