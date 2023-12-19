using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using WebShop.Client.Code;
using WebShop.Client.Services;
using WebShop.DTO.Input;
using WebShop.DTO.Output;

namespace WebShop.Client.Pages
{
    public partial class SubscriptionPlan
    {
        [Inject]
        private IApiServices ApiServices { get; set; }

        [Inject]
        protected GlobalUserSettings GlobalSettings { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        private List<SubscriptionPlanODTO> subscriptionPlans = new();

        private bool isLoading = false;

        private string userId;
        private Dictionary<int, bool> isPaymentInProgress;

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;

            subscriptionPlans = await ApiServices.GetSubscriptionPlansAsync();
            isPaymentInProgress = subscriptionPlans.ToDictionary(x => x.SubscriptionPlanId, x => false);

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
            isPaymentInProgress[subscriptionPlanId] = true;

            var userSubscriptionPlanIDTO = new UserSubscriptionPlanIDTO(GlobalSettings.UserId!)
            {
                SubscriptionPlanId = subscriptionPlanId
            };

            var redirectUrl = await ApiServices.ChooseSubscriptionPlanAsync(userSubscriptionPlanIDTO);
            NavigationManager.NavigateTo(redirectUrl.RedirectUrl);

            isPaymentInProgress[subscriptionPlanId] = false;
        }
    }
}
