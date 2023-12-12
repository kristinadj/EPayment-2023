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
        public string UserId { get; set; }

        [Parameter]
        public int PaymentMethodId { get; set; }

        async void Submit()
        {
            var isSuccess = await ApiServices.UnsubscribeFromPaymentMethodAsync(PaymentMethodId, UserId);
            MudDialog.Close(DialogResult.Ok(isSuccess));
        }

        void Cancel() => MudDialog.Cancel();
    }
}
