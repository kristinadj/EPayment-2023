using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using WebShop.Client.Code;
using WebShop.Client.Services;
using WebShop.DTO.Enums;
using WebShop.DTO.Output;

namespace WebShop.Client.Pages
{
    public partial class Index
    {
        [Inject]
        private IApiServices ApiServices { get; set; }

        [Inject]
        protected GlobalUserSettings GlobalSettings { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        private bool isLoading = false;
        private bool isMerchantRegistered = false;
        private bool isRegistrationInProgress = false;
        private List<ItemODTO> items { get; set; } = new();
        private List<PaymentMethodMerchantODTO> paymentMethods { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;

            var user = (await AuthenticationStateTask).User;
            if (user != null && user.Identity!.IsAuthenticated)
            {
                var userId = user.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
                if (userId != null)
                {
                    GlobalSettings.UserId = userId.Value;

                    var shoppingCart = await ApiServices.GetShoppingCartByUserAsync(GlobalSettings.UserId);
                    if (shoppingCart != null)
                    {
                        GlobalSettings.ShoppingCartId = shoppingCart.ShoppingCartId;
                        GlobalSettings.ShoppingCartItemsCount = shoppingCart.ShoppingCartItems!.Select(x => x.Quantity).Sum();
                    }
                }

                var role = user.Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault();
                if (role != null)
                {
                    GlobalSettings.Role = role.Value.ToString() == Role.BUYER.ToString() ? Role.BUYER : Role.MERCHANT;
                }
            }
            else
            {
                Navigation!.NavigateTo("/login");
            }

            if (GlobalSettings.Role == Role.BUYER)
            {
                items = await ApiServices.GetItemsAsync();
            }
            else
            {
                isMerchantRegistered = await ApiServices.IsMerchantRegisteredOnPspAsync(GlobalSettings.UserId!);

                if (isMerchantRegistered)
                {
                    paymentMethods = await ApiServices.GetPaymentMethodsByUserIdAsync(GlobalSettings.UserId!);
                }
            }

            isLoading = false;
        }

        private void OnUnsubscribeButtonClicked(int paymentMethodMerchantId)
        {

        }

        private async void OnRegisterOnPspClicked() 
        {
            isRegistrationInProgress = true;
            var isSuccess = await ApiServices.RegisterMerchantOnPspAsync(GlobalSettings.UserId!);
            if (isSuccess)
            {
                paymentMethods = await ApiServices.GetPaymentMethodsByUserIdAsync(GlobalSettings.UserId!);
                StateHasChanged();
            }
            isRegistrationInProgress = false;
        }
    }
}
