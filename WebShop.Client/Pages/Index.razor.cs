using Microsoft.AspNetCore.Components;
using WebShop.Client.Services;
using WebShop.DTO.Output;

namespace WebShop.Client.Pages
{
    public partial class Index
    {
        [Inject]
        private IApiServices ApiServices { get; set; }

        private bool isLoading = false;
        private List<ItemODTO> items { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            items = await ApiServices.GetItemsAsync();
            isLoading = false;
        }
    }
}
