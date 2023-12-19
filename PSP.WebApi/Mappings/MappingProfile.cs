using AutoMapper;
using Base.DTO.Shared;
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
                .ConstructUsing(x => new Merchant(x.MerchantExternalId, x.Name, x.Address, x.PhoneNumber, x.Email, x.ServiceName, x.TransactionSuccessUrl, x.TransactionFailureUrl, x.TransactionErrorUrl));

            CreateMap<Merchant, MerchantODTO>()
                    .ConstructUsing(x => new MerchantODTO(x.MerchantExternalId, x.Name, x.Address, x.PhoneNumber, x.Email, x.ServiceName, x.TransactionSuccessUrl, x.TransactionFailureUrl, x.TransactionErrorUrl));

            CreateMap<PspInvoiceIDTO, InvoiceODTO>();
            CreateMap<Invoice, InvoiceODTO>()
                .ForMember(x => x.CurrencyCode, x=> x.MapFrom(x => x.Currency!.Code));

            CreateMap<PaymentMethod, PaymentMethodMerchantODTO>()
                .ForMember(x => x.PaymentMethodMerchantId, x => x.MapFrom(x => 0))
                .ForMember(x => x.IsActive, x => x.MapFrom(x => false))
                .ForMember(x => x.PaymentMethod, x => x.MapFrom(x => x));

            CreateMap<PaymentMethodMerchant, PaymentMethodODTO>()
               .ForMember(x => x.PaymentMethodId, x => x.MapFrom(x => x.PaymentMethod!.PaymentMethodId))
               .ConstructUsing(x => new PaymentMethodODTO(x.PaymentMethod!.Name, x.PaymentMethod!.ServiceName, x.PaymentMethod!.ServiceApiSufix));

            CreateMap<PaymentMethodMerchant, PaymentMethodMerchantODTO>();
        }
    }
}
