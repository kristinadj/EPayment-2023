using AutoMapper;
using PSP.WebApi.DTO.Input;
using PSP.WebApi.DTO.Output;
using PSP.WebApi.Models;

namespace PSP.WebApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PaymentMethodIDTO, PaymentMethod>()
                .ConstructUsing(x => new PaymentMethod(x.Name, x.ServiceName, x.ServiceApiSufix));

            CreateMap<PaymentMethod, PaymentMethodODTO>()
                .ConstructUsing(x => new PaymentMethodODTO(x.Name, x.ServiceName, x.ServiceApiSufix));
        }
    }
}
