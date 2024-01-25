using Base.DTO.Enums;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebShop.Client.Services;
using WebShop.DTO.Output;

namespace WebShop.Client.Pages
{
    public partial class InvoiceSuccess
    {
        [Inject]
        private IApiServices ApiServices { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Parameter]
        public int InvoiceId { get; set; }

        [Parameter]
        public string? ExternalSubscriptionId { get; set; }

        private bool isLoading = false;
        private bool unexpectedError = false;
        private bool isPaymentInProgress = false;

        private InvoiceODTO? invoice;
        private OrderODTO? order;

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;

            invoice = await ApiServices.GetInvoiceByIdAsync(InvoiceId);
            if (invoice == null)
            {
                unexpectedError = true;
            }

            var isSuccess = await ApiServices.UpdateTransactionStatusAsync(invoice!.Transaction!.TransactionId, DTO.Enums.TransactionStatus.COMPLETED);
            if (!isSuccess)
            {
                unexpectedError = true;
            }
            else
            {
                if (invoice.InvoiceType == InvoiceType.ORDER)
                {
                    order = await ApiServices.GetOrderByInvoiceIdAsync(InvoiceId);
                    if (order == null) unexpectedError = true;
                }
            }

            if (!string.IsNullOrEmpty(ExternalSubscriptionId))
            {
                await ApiServices.UpdateExternalSubscriptionIdAsync(InvoiceId, ExternalSubscriptionId);
            }

            isLoading = false;
        }

        private async Task OnClickPayAsync(int merchantOrderId)
        {
            isPaymentInProgress = true;
            var result = await ApiServices.CreateInvoiceAsync(merchantOrderId);

            if (result != null)
            {
                NavigationManager.NavigateTo(result.RedirectUrl);
            }

            isPaymentInProgress = false;
        }

        private void OnClickContinueShopping()
        {
            NavigationManager.NavigateTo("/");
        }
    }
}
