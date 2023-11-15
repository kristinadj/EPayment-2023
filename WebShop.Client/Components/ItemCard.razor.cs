using Microsoft.AspNetCore.Components;
using WebShop.DTO;

namespace WebShop.Client.Components
{
    public partial class ItemCard
    {
        [Parameter]
        public ItemDTO? Item { get; set; }

        private int quantity = 1;
    }
}
