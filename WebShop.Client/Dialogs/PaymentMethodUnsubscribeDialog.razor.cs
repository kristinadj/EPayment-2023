using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebShop.Client.Services;

namespace WebShop.Client.Dialogs
{
    public partial class PaymentMethodUnsubscribeDialog
    {
        [Inject]
        private IApiServices ApiServices { get; set; }

        [CascadingParameter] MudDialogInstance MudDialog { get; set; }

        [Parameter]
        public int PaymentMethodMerchantId { get; set; }

        async void Submit()
        {
            var isSuccess = true;
            MudDialog.Close(DialogResult.Ok(isSuccess));
        }

        void Cancel() => MudDialog.Cancel();
    }
}
