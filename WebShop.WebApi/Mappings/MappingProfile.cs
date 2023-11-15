using AutoMapper;
using WebShop.DTO.Input;
using WebShop.DTO.Output;
using WebShop.WebApi.Models;

namespace WebShop.WebApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        { 
            CreateMap<UserIDTO, User>()
                .ForMember(d => d.UserName, s => s.MapFrom(x => x.Email))
                .ConstructUsing(x => new User($"{x.FirstName} {x.LastName}"));

            CreateMap<Currency, CurrencyODTO>();

            CreateMap<SubscriptionPlan, SubscriptionPlanODTO>();

            CreateMap<Item, ItemODTO>();

            CreateMap<Merchant, MerchantODTO>()
                .ConstructUsing(x => new MerchantODTO(x.User!.Id, x.User.Name, x.User.Address!, x.User.PhoneNumber , x.User.Email));

            CreateMap<ShoppingCart, ShoppingCartODTO>();

            CreateMap<ShoppingCartItem, ShoppingCartItemODTO>();
        }
    }
}
