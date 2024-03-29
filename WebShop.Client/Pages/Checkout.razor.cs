﻿using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebShop.Client.Services;
using WebShop.DTO.Output;

namespace WebShop.Client.Pages
{
    public partial class Checkout
    {
        [Inject]
        private IApiServices ApiServices { get; set; }

        [Inject]
        private ISnackbar Snackbar { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Parameter]
        public int OrderId { get; set; }

        private bool isLoading = false;
        private bool isPaymentInProgress = false;

        private OrderODTO? order;

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;

            order = await ApiServices.GetOrderByIdAsync(OrderId);
            isLoading = false;
        }

        private async Task OnClickCancelAsync()
        {
            var result = await ApiServices.CancelOrderAsync(OrderId);

            if (result != null)
            {
                Snackbar.Add("Order successfully canceled", Severity.Success);
                NavigationManager.NavigateTo("/shoppingCart");
            }
        }

        private async Task OnClickPayAsync(int merchantOrderId)
        {
            isPaymentInProgress = true;
            var result = await ApiServices.CreateInvoiceAsync(merchantOrderId);

            if (result != null)
            {
                NavigationManager.NavigateTo(result.RedirectUrl);
            }

            isPaymentInProgress = false;
        }
    }
}
