using Bank1.Client.Services;
using Base.DTO.NBS;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bank1.Client.Pages
{
    public partial class QrCodePayment
    {
        [Inject]
        private IApiServices ApiServices { get; set; }

        [Inject]
        private ISnackbar Snackbar { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Parameter]
        public int TransactionId { get; set; }

        private string? qrCodeUrl;
        private string? qrCodeInput;

        public bool isLoading = false;

        public bool isValidating = false;
        public bool isValid = false;
        public bool showValidationResult = false;
        public string validationResult = string.Empty;

        public bool isRefreshing = false;

        protected async override Task OnInitializedAsync()
        {
            isLoading = true;

            var qrCodeBase64 = await ApiServices.GenerateQrCodeAsync(TransactionId);
            qrCodeUrl = string.Format("data:image/jpeg;base64,{0}", qrCodeBase64);

            qrCodeInput = await ApiServices.GetQrCodeInputAsync(TransactionId);

            if (string.IsNullOrEmpty(qrCodeUrl) || string.IsNullOrEmpty(qrCodeInput))
            {
                Snackbar.Add("Unexpected error occurred", Severity.Error);
            }

            isLoading = false;
        }

        private async Task OnClickValidateAsync()
        {
            isValidating = true;

            var qrCode = await ApiServices.ValdiateQrCodeAsync(new BankvalidateQrCodeIDTO { Input = qrCodeInput! });
            if (qrCode == null)
            {
                isValid = false;
                validationResult = "Unexpected error during validation";
            }
            else
            {
                if (qrCode.Status!.Code != 0)
                {
                    isValid = false;
                }
                else
                {
                    isValid = true;
                }

                validationResult = qrCode.Status!.Description;
            }

            showValidationResult = true;
            isValidating = false;
        }

        private async Task OnClickRefreshAsync()
        {
            isRefreshing = true;

            var result = await ApiServices.GetTransactionStatusAsync(TransactionId);
            if (result != null)
            {
                NavigationManager.NavigateTo(result.RedirectUrl);
            }

            isRefreshing = false;
        }
    }
}
