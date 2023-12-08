using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using WebShop.Client.Services;
using WebShop.DTO.Output;

namespace WebShop.Client.Pages
{
    public partial class SubscriptionPlan
    {
        [Inject]
        private IApiServices ApiServices { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        private List<SubscriptionPlanODTO> subscriptionPlans = new();
        private List<PaymentMethodODTO> paymentMethods = new();

        private bool isLoading = false;

        private string userId;
        private bool isPaymentInProgress = false;
        private int selectedPaymentMethod;

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;

            subscriptionPlans = await ApiServices.GetSubscriptionPlansAsync();
            paymentMethods = await ApiServices.GetPaymentMethodsAsync();
            if (paymentMethods.Any())
            {
                selectedPaymentMethod = paymentMethods.First().PaymentMethodId;
            }

            var user = (await AuthenticationStateTask).User;
            if (user != null && user.Identity!.IsAuthenticated)
            {
                var tempUserId = user.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
                if (tempUserId != null)
                {
                    userId = tempUserId.Value;
                }
            }

            isLoading = false;
        }

        private async void OnChooseSubscriptionPlanAsync(int subscriptionPlanId)
        {

        }
    }
}
