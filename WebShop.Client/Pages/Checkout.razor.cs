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

        [Parameter]
        public int OrderId { get; set; }

        private OrderODTO? order;
        private List<PaymentMethodODTO> paymentMethods = new();

        private int selectedPaymentMethod { get; set; }

        protected override async Task OnInitializedAsync()
        {
            paymentMethods = await ApiServices.GetPaymentMethodsAsync();
            if (paymentMethods.Any())
            {
                selectedPaymentMethod = paymentMethods.First().PaymentMethodId;
            }
        }

        private async Task OnClickPayAsync()
        {
            var result = await ApiServices.CreateInvoiceAsync(OrderId, (int)selectedPaymentMethod);

            if (result != null)
            {
                Snackbar.Add(result.RedirectUrl, Severity.Success);
            }
            else
            {
                Snackbar.Add("Error", Severity.Error);
            }
        }
    }
}
