using Bank.DTO.Input;
using Bank1.Client.QrCode;
using Bank1.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MudBlazor;
using System.Security.Claims;

namespace Bank1.Client.Pages
{
    public partial class Index
    {
        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        [Inject]
        private IAuthService AuthService { get; set; }

        [Inject]
        private IApiServices ApiServices { get; set; }

        [Inject]
        private NavigationManager Navigation { get; set; }

        [Inject]
        private ISnackbar Snackbar { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        #region Login

        private MudForm? form = new();
        private AuthenticateIDTO authenticateDTO = new();
        private bool isValid;
        private string[] errors = { };

        private bool showPassword = false;
        private InputType passwordInputType = InputType.Password;
        private string passwordInputIcon = Icons.Material.Filled.VisibilityOff;

        private bool isLoading = false;
        private string userId = string.Empty;
        #endregion

        #region QRScanner
        private QRCodeScannerJsInterop? qrCodeScannerJsInterop;
        private Action<string>? onQrCodeScanAction;
        private string? scannedValue;

        private bool isPaid = false;
        private bool showCamera = false;

        #endregion

        protected override async Task OnInitializedAsync()
        {
            var user = (await AuthenticationStateTask).User;
            if (user != null && user.Identity!.IsAuthenticated)
            {
                var userIdClaim = user.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
                if (userIdClaim != null)
                {
                    userId = userIdClaim!.Value;
                    showCamera = true;
                }
            }

            onQrCodeScanAction = (code) => OnQrCodeScan(code);
            qrCodeScannerJsInterop = new QRCodeScannerJsInterop(JSRuntime);
            await qrCodeScannerJsInterop.Init(onQrCodeScanAction);
        }

        private void OnQrCodeScan(string code)
        {
            scannedValue = code;
            StateHasChanged();
        }

        private async Task OnClickPay()
        {
            isLoading = true;
            var qrCodePayment = new QrCodePaymentIDTO(userId, scannedValue!);
            isPaid = await ApiServices.PayQrCodeAsync(qrCodePayment);
            
            if (isPaid)
            {
                Snackbar.Add("Successfully paid", Severity.Success);
            }

            isLoading = false;
            StateHasChanged();
        }

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
            if (isValid && authenticateDTO != null)
            {
                isLoading = true;
                var result = await AuthService.Login(authenticateDTO);
                if (result != null)
                {
                    showCamera = true;
                    isLoading = false;
                }
            }

            isLoading = false;
            StateHasChanged();
        }

        private void OnClickLogout(MouseEventArgs e)
        {
            showCamera = false;
            Navigation!.NavigateTo("/logout");
        }
    }
}
