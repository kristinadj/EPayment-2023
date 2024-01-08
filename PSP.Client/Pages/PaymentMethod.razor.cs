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
        public int InvoiceId { get; set; }

        [Parameter]
        public bool RecurringPayment { get; set; }

        private InvoiceODTO? invoice { get; set; }
        private List<PaymentMethodODTO> paymentMethods = new();

        private bool isLoading = false;
        private bool isRedirectInProgress = false;
        private int selectedPaymentMethod { get; set; }

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;

            invoice = await ApiServices.GetInvoiceByIdAsync(InvoiceId);

            if (invoice != null)
            {
                paymentMethods = await ApiServices.GetPaymentMethodsAsync(invoice.MerchantId, RecurringPayment);
                if (paymentMethods.Any())
                {
                    selectedPaymentMethod = paymentMethods.First().PaymentMethodId;
                }
            }
            

            isLoading = false;
        }

        private async Task OnClickedContinueAsync()
        {
            isRedirectInProgress = true;

            var redirectUrl = await ApiServices.UpdatePaymentMethodAsync(InvoiceId, selectedPaymentMethod);

            if (redirectUrl != null)
                NavigationManager.NavigateTo(redirectUrl.RedirectUrl);
            else
                Snackbar.Add("Error", Severity.Error);

            isRedirectInProgress = false;
        }
    }
}