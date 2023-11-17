using Microsoft.AspNetCore.Components;
using WebShop.Client.Services;
using WebShop.DTO.Output;

namespace WebShop.Client.Pages
{
    public partial class InvoiceError
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

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;

            invoice = await ApiServices.GetInvoiceByIdAsync(InvoiceId);
            if (invoice == null) unexpectedError = true;

            var isSuccess = await ApiServices.UpdateTransactionStatusAsync(invoice!.Transaction!.TransactionId, DTO.Enums.TransactionStatus.ERROR);
            if (!isSuccess) unexpectedError = true;

            isLoading = false;
        }

        private void OnClickContinueShopping()
        {
            NavigationManager.NavigateTo("/");
        }
    }
}
