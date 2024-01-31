using AutoMapper.QueryableExtensions;
using AutoMapper;
using WebShop.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using WebShop.DTO.Output;

namespace WebShop.WebApi.Services
{
    public interface IItemService
    {
        Task<List<ItemODTO>> GetItemsAsync();
    }

    public class ItemServices : IItemService
    {
        private readonly WebShopContext _context; 
        private readonly IMapper _mapper;
        
        public ItemServices(WebShopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ItemODTO>> GetItemsAsync()
        {
            return await _context.Items
                .Where(x => x.Merchant!.PspMerchantId != null)
                .ProjectTo<ItemODTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
