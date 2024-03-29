﻿using Microsoft.AspNetCore.Components;
using MudBlazor.Interfaces;
using WebShop.Client.Code;
using WebShop.Client.Services;
using WebShop.DTO.Output;

namespace WebShop.Client.Pages
{
    public partial class BuyerInvoicesArchive
    {
        [Inject]
        private IApiServices ApiServices { get; set; }

        [Inject]
        protected GlobalUserSettings GlobalSettings { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private bool isLoading = false;
        private List<InvoiceODTO> invoices = new();

        protected override async Task OnInitializedAsync()
        {
            if (GlobalSettings.IsSubscriptionPlanValid == null || !(bool)GlobalSettings.IsSubscriptionPlanValid)
            {
                NavigationManager!.NavigateTo("/plan");
            }
            else
            {
                isLoading = true;
                invoices = await ApiServices.GetBuyerInvoicesAsync(GlobalSettings.UserId!);
                isLoading = false;
                StateHasChanged();
            }
        }
    }
}
