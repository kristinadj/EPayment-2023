using Base.DTO.Output;
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

        private List<InstitutionODTO> institutions = new();

        private PaymentMethodSubscribeIDTO paymentMethodSubscribe = new();

        private bool isLoading = false;
        private bool isSubscribing = false;
        private MudForm? form = new();
        private bool isValid;

        private bool showPassword = false;
        private InputType passwordInputType = InputType.Password;
        private string passwordInputIcon = Icons.Material.Filled.VisibilityOff;

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;

            institutions = await ApiServices.GetPaymentMethodInstitutionsAsync(PaymentMethodId);
            if (institutions.Count != 0)
            {
                paymentMethodSubscribe.InstitutionId = institutions.First().InstitutionId;
            }

            isLoading = false;
        }

        async void Submit()
        {
            if (isValid) 
            {
                isSubscribing = true;
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
