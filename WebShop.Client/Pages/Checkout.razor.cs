using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebShop.Client.Services;
using WebShop.DTO.Output;

namespace WebShop.Client.Pages
{
    public partial class Checkout
    {
        [Inject]
        private IApiServices ApiServices { get; set; }

        [Inject]
        private ISnackbar Snackbar { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Parameter]
        public int OrderId { get; set; }

        private bool isLoading = false;
        private bool isPaymentInProgress = false;

        private OrderODTO? order;
        private List<PaymentMethodODTO> paymentMethods = new();

        private int selectedPaymentMethod { get; set; }

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;

            order = await ApiServices.GetOrderByIdAsync(OrderId);
            paymentMethods = await ApiServices.GetPaymentMethodsAsync();
            if (paymentMethods.Any())
            {
                selectedPaymentMethod = paymentMethods.First().PaymentMethodId;
            }

            isLoading = false;
        }

        private async Task OnClickCancelAsync()
        {
            var result = await ApiServices.CancelOrderAsync(OrderId);

            if (result != null)
            {
                Snackbar.Add("Order successfully canceled", Severity.Success);
                NavigationManager.NavigateTo("/shoppingCart");
            }
            else
            {
                Snackbar.Add("Error", Severity.Error);
            }
        }

        private async Task OnClickPayAsync()
        {
            isPaymentInProgress = true;
            var result = await ApiServices.CreateInvoiceAsync(OrderId, (int)selectedPaymentMethod);

            if (result != null)
            {
                NavigationManager.NavigateTo(result.RedirectUrl);
            }
            else
            {
                Snackbar.Add("Error", Severity.Error);
            }
            isPaymentInProgress = false;
        }
    }
}
