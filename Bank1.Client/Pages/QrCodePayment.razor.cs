using Bank1.Client.Services;
using Base.DTO.Input;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bank1.Client.Pages
{
    public partial class QrCodePayment
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private IApiServices ApiServices { get; set; }

        [Inject]
        private ISnackbar Snackbar { get; set; }

        [Parameter]
        public int TransactionId { get; set; }

        private string? qrCodeSvg;
        public bool isLoading = false;

        protected async override Task OnInitializedAsync()
        {
            isLoading = true;

            qrCodeSvg = await ApiServices.GenerateQrCodeAsync(TransactionId);

            if (string.IsNullOrEmpty(qrCodeSvg))
            {
                Snackbar.Add("Unexpected error occurred", Severity.Error);
                
            }

            isLoading = false;
        }
    }
}
