using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using WebShop.Client.Authentication;
using WebShop.Client.Code;
using WebShop.DTO.Input;

namespace WebShop.Client.Pages
{
    public partial class Login
    {
        [Inject]
        private IAuthService AuthService { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private ISnackbar Snackbar { get; set; }

        private MudForm? form = new();
        private AuthenticateIDTO authenticateDTO = new();
        private bool isValid;
        private string[] errors = { };

        private bool showPassword = false;
        private InputType passwordInputType = InputType.Password;
        private string passwordInputIcon = Icons.Material.Filled.VisibilityOff;

        private bool isLoading = false;

        void OnShowPassword()
        {
            if (showPassword)
            {
                showPassword = false;
                passwordInputIcon = Icons.Material.Filled.VisibilityOff;
                passwordInputType = InputType.Password;
            }
            else
            {
                showPassword = true;
                passwordInputIcon = Icons.Material.Filled.Visibility;
                passwordInputType = InputType.Text;
            }
        }

        private async void OnLoginAsync()
        {
            try
            {
                if (isValid && authenticateDTO != null)
                {
                    isLoading = true;
                    var result = await AuthService.Login(authenticateDTO);
                    if (result != null)
                    {
                        NavigationManager.NavigateTo("/");
                    }
                    else
                    {
                        Snackbar.Add("Bad Credentials", Severity.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add("Unexpected Exception", Severity.Error);
            }

            isLoading = false;
            StateHasChanged();
        }
    }
}
