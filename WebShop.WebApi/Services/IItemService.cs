using AutoMapper.QueryableExtensions;
using AutoMapper;
using WebShop.DTO;
using WebShop.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace WebShop.WebApi.Services
{
    public interface IItemService
    {
        Task<List<ItemDTO>> GetItemsAsync();
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

        public async Task<List<ItemDTO>> GetItemsAsync()
        {
            return await _context.Items
                .ProjectTo<ItemDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
