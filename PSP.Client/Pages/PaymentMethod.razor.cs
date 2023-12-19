using Microsoft.AspNetCore.Components;
using MudBlazor;
using PSP.Client.DTO.Output;
using PSP.Client.Services;

namespace PSP.Client.Pages
{
    public partial class PaymentMethod
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private IApiServices ApiServices { get; set; }

        [Inject]
        private ISnackbar Snackbar { get; set; }

        [Parameter]
        public int MerchantId { get; set; }

        private List<PaymentMethodODTO> paymentMethods = new();

        private bool isLoading = false;
        private bool isRedirectInProgress = false;
        private int selectedPaymentMethod { get; set; }

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;

            paymentMethods = await ApiServices.GetPaymentMethodsAsync(MerchantId);
            if (paymentMethods.Any())
            {
                selectedPaymentMethod = paymentMethods.First().PaymentMethodId;
            }

            isLoading = false;
        }
    }
}