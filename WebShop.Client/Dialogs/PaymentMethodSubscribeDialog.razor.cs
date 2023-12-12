using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebShop.Client.Services;
using WebShop.DTO.Input;

namespace WebShop.Client.Dialogs
{
    public partial class PaymentMethodSubscribeDialog
    {
        [Inject]
        private IApiServices ApiServices { get; set; }

        [Inject]
        private ISnackbar Snackbar { get; set; }

        [CascadingParameter] MudDialogInstance MudDialog { get; set; }

        [Parameter]
        public string UserId { get; set; }

        [Parameter]
        public int PaymentMethodId { get; set; }

        private PaymentMethodSubscribeIDTO paymentMethodSubscribe = new();

        private bool isSubscribing { get; set; }
        private MudForm? form = new();
        private bool isValid;

        private bool showPassword = false;
        private InputType passwordInputType = InputType.Password;
        private string passwordInputIcon = Icons.Material.Filled.VisibilityOff;

        async void Submit()
        {
            if (!int.TryParse(paymentMethodSubscribe.StrCode, out int tempCode))
            {
                Snackbar.Add("Code must be a numberic field", Severity.Warning);
            }
            else if (isValid) 
            {
                isSubscribing = true;
                paymentMethodSubscribe.Code = tempCode;
                paymentMethodSubscribe.UserId = UserId;
                paymentMethodSubscribe.PaymentMethodId = PaymentMethodId;
                var isSuccess = await ApiServices.SubscribeToPaymentMethodAsync(paymentMethodSubscribe);

                isSubscribing = false;
                MudDialog.Close(DialogResult.Ok(isSuccess));
            }
        }

        void Cancel() => MudDialog.Cancel();

        void OnShowPassword()
        {
            if (showPassword)
            {
                showPassword = false;
                passwordInputIcon = Icons.Material.Filled.VisibilityOff;
                passwordInputType = InputType.Password;
            }
            else
            {
                showPassword = true;
                passwordInputIcon = Icons.Material.Filled.Visibility;
                passwordInputType = InputType.Text;
            }
        }
    }
}
