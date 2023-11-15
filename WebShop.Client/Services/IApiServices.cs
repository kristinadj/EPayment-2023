using System.Runtime.CompilerServices;
using WebShop.DTO;

namespace WebShop.Client.Services
{
    public interface IApiServices
    {
        Task<List<ItemDTO>> GetItemsAsync();
    }


}
