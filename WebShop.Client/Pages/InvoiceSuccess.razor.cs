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

        private bool isLoading = false;
        private bool unexpectedError = false;

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
            else if (invoice.InvoiceType == DTO.Enums.InvoiceType.ORDER)
            {
                order = await ApiServices.GetOrderByInvoiceIdAsync(InvoiceId);
                if (order == null) unexpectedError = true;
            }

            var isSuccess = await ApiServices.UpdateTransactionStatusAsync(invoice!.Transaction!.TransactionId, DTO.Enums.TransactionStatus.COMPLETED);
            if (!isSuccess) unexpectedError = true;

            isLoading = false;
        }

        private void OnClickContinueShopping()
        {
            NavigationManager.NavigateTo("/");
        }
    }
}
