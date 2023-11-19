using Bank1.Client.DTO.Input;
using Bank1.Client.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Headers;

namespace Bank1.Client.Pages
{
    public partial class Payment
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private IApiServices ApiServices { get; set; }

        [Inject]
        private ISnackbar Snackbar { get; set; }

        [Parameter]
        public int TransactionId { get; set; }

        private string cardHolderName = string.Empty;
        private string panNumber = string.Empty;
        private string expiration = string.Empty;
        private string cvv = string.Empty;

        public bool isLoading = false;

        private async Task OnPayAsync()
        {
            isLoading = true;
            var request = new PayTransactionIDTO(cardHolderName, panNumber, expiration)
            {
                TransactionId = TransactionId,
                CVV = int.Parse(cvv)
            };

            var result = await ApiServices.PayTransactionAsync(request);
            if (result == null)
            {
                Snackbar.Add("Unexpected error occurred", Severity.Error);
                isLoading = false;
            }
            else
            {
                NavigationManager.NavigateTo(result.RedirectUrl);
            }
        }
    }
}
