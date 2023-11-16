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

            CreateMap<MerchantIDTO, Merchant>()
                .ConstructUsing(x => new Merchant(x.Name, x.ServiceName, x.TransactionSuccessUrl, x.TransactionFailureUrl, x.TransactionErrorUrl));

            CreateMap<Merchant, MerchantODTO>()
                    .ConstructUsing(x => new MerchantODTO(x.Name, x.ServiceName, x.TransactionSuccessUrl, x.TransactionFailureUrl, x.TransactionErrorUrl));
        }
    }
}
