using Microsoft.AspNetCore.Components;
using WebShop.Client.Code;
using WebShop.Client.Services;
using WebShop.DTO.Output;

namespace WebShop.Client.Pages
{
    public partial class MerchantInvoicesArchive
    {
        [Inject]
        private IApiServices ApiServices { get; set; }

        [Inject]
        protected GlobalUserSettings GlobalSettings { get; set; }

        private bool isLoading = false;
        private List<InvoiceODTO> invoices = new();

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            invoices = await ApiServices.GetMerchantInvoicesAsync(GlobalSettings.UserId!);
            isLoading = false;
            StateHasChanged();
        }
    }
}