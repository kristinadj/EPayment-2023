using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text.RegularExpressions;
using WebShop.Client.Authentication;
using WebShop.DTO.Input;

namespace WebShop.Client.Pages
{
    public partial class Register
    {
        [Inject]
        private IAuthService AuthService { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private ISnackbar Snackbar { get; set; }

        private MudForm? form = new();
        private UserIDTO userDTO = new();
        private bool isValid;
        private string[] errors = { };

        private bool showPassword = false;
        private InputType passwordInputType = InputType.Password;
        private string passwordInputIcon = Icons.Material.Filled.VisibilityOff;

        private bool showConfirmPassword = false;
        private InputType confirmPasswordInputType = InputType.Password;
        private string confirmPasswordInputIcon = Icons.Material.Filled.VisibilityOff;

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

        void OnShowConfirmPassword()
        {
            if (showConfirmPassword)
            {
                showConfirmPassword = false;
                confirmPasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                confirmPasswordInputType = InputType.Password;
            }
            else
            {
                showConfirmPassword = true;
                confirmPasswordInputIcon = Icons.Material.Filled.Visibility;
                confirmPasswordInputType = InputType.Text;
            }
        }

        private IEnumerable<string> PasswordStrength(string pw)
        {
            if (string.IsNullOrWhiteSpace(pw))
            {
                yield return "Password is required!";
                yield break;
            }
            if (pw.Length < 6)
                yield return "Password must be at least of length 6";
            if (!Regex.IsMatch(pw, @"[A-Z]"))
                yield return "Password must contain at least one capital letter";
            if (!Regex.IsMatch(pw, @"[a-z]"))
                yield return "Password must contain at least one lowercase letter";
            if (!Regex.IsMatch(pw, @"[0-9]"))
                yield return "Password must contain at least one digit";
        }

        private string? PasswordMatch(string arg)
        {
            if (userDTO.Password != arg)
                return "Passwords don't match";
            return null;
        }

        private async void OnRegisterAsync()
        {
            try
            {
                if (isValid && userDTO != null)
                {
                    isLoading = true;
                    var authResult = await AuthService.Register(userDTO);
                    if (authResult == null)
                    {
                        Snackbar.Add("Email is taken", Severity.Error);
                    }
                    else
                    {
                        NavigationManager.NavigateTo("/plan");
                    }
                }
            }
            catch (Exception)
            {
                Snackbar.Add("Unexpected Exception", Severity.Error);
            }

            isLoading = false;
            StateHasChanged();
        }
    }
}
