using AutoMapper;
using WebShop.DTO;
using WebShop.WebApi.Models;

namespace WebShop.WebApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        { 
            CreateMap<UserDTO, User>()
                .ForMember(d => d.UserName, s => s.MapFrom(x => x.Email))
                .ConstructUsing(x => new User($"{x.FirstName} {x.LastName}"));

            CreateMap<Currency, CurrencyDTO>();

            CreateMap<SubscriptionPlan, SubscriptionPlanDTO>();

            CreateMap<Item, ItemDTO>();

            CreateMap<Merchant, MerchantDTO>()
                .ConstructUsing(x => new MerchantDTO(x.User!.Id, x.User.Name, x.User.Address!, x.User.PhoneNumber , x.User.Email));
        }
    }
}
