using AutoMapper;
using Base.DTO.Shared;
using WebShop.DTO.Enums;
using WebShop.DTO.Input;
using WebShop.DTO.Output;
using WebShop.WebApi.Models;
using TransactionLog = WebShop.WebApi.Models.TransactionLog;

namespace WebShop.WebApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserIDTO, User>()
                .ForMember(d => d.UserName, s => s.MapFrom(x => x.Email))
                .ConstructUsing(x => new User($"{x.FirstName} {x.LastName}"));
            CreateMap<User, UserODTO>();

            CreateMap<Currency, CurrencyODTO>();

            CreateMap<SubscriptionPlan, SubscriptionPlanODTO>();
            CreateMap<UserSubscriptionPlan, UserSubscriptionPlanODTO>();

            CreateMap<Item, ItemODTO>();

            CreateMap<Merchant, MerchantODTO>()
                .ConstructUsing(x => new MerchantODTO(x.User!.Id, x.User.Name, x.User.Address!, x.User.PhoneNumber!, x.User.Email!));

            CreateMap<ShoppingCart, ShoppingCartODTO>();

            CreateMap<ShoppingCartItem, ShoppingCartItemODTO>();

            CreateMap<ShoppingCart, Order>()
                .ForMember(x => x.OrderStatus, x => x.MapFrom(x => OrderStatus.CREATED))
                .ForMember(x => x.CreatedTimestamp, x => x.MapFrom(x => DateTime.Now))
                .ForMember(x => x.OrderLogs, x => x.MapFrom(x => new List<OrderLog> { new() { OrderStatus = OrderStatus.CREATED, Timestamp = DateTime.Now } }))
                .ConstructUsing(x => new Order(x.UserId));

            CreateMap<ShoppingCartItem, OrderItem>()
                .ForMember(x => x.Price, x => x.MapFrom(x => x.Item!.Price))
                .ForMember(x => x.CurrencyId, x => x.MapFrom(x => x.Item!.CurrencyId));

            CreateMap<Order, OrderODTO>()
                .ConstructUsing(x => new OrderODTO(x.UserId));

            CreateMap<MerchantOrder, MerchantOrderODTO>();
            CreateMap<OrderItem, OrderItemODTO>();
            CreateMap<OrderLog, OrderLogODTO>();
            
            CreateMap<PaymentMethodDTO, PaymentMethod>()
                .ForMember(x => x.PaymentMethodId, x => x.MapFrom(x => 0))
                .ForMember(x => x.PspPaymentMethodId, x => x.MapFrom(x => x.PaymentMethodId))
                .ConstructUsing(x => new PaymentMethod($"{x.ServiceName}/{x.ServiceApiSufix}", x.ServiceName));

            CreateMap<Invoice, InvoiceODTO>()
                .ForMember(x => x.Timestamp, x => x.MapFrom(x => x.Transaction!.CreatedTimestamp));

            CreateMap<Transaction, TransactionODTO>();
            CreateMap<TransactionLog, TransactionLogODTO>();

            CreateMap<PaymentMethod, PaymentMethodODTO>();

            CreateMap<InvoiceODTO, PspInvoiceIDTO>()
                .ForMember(x => x.ExternalInvoiceId, x => x.MapFrom(x => x.InvoiceId))
                .ForMember(x => x.MerchantId, x => x.MapFrom(x => x.Merchant!.PspMerchantId))
                .ConstructUsing(x => new PspInvoiceIDTO(x!.UserId, x.Currency!.Code));

            CreateMap<Invoice, PspInvoiceIDTO>()
                .ForMember(x => x.ExternalInvoiceId, x => x.MapFrom(x => x.InvoiceId))
                .ForMember(x => x.MerchantId, x => x.MapFrom(x => x.Merchant!.PspMerchantId))
                .ConstructUsing(x => new PspInvoiceIDTO(x!.UserId, x.Currency!.Code));

            CreateMap<Invoice, PspSubscriptionPaymentDTO>()
                .ForMember(x => x.ExternalInvoiceId, x => x.MapFrom(x => x.InvoiceId))
                .ForMember(x => x.MerchantId, x => x.MapFrom(x => x.Merchant!.PspMerchantId))
                .ConstructUsing(x => new PspSubscriptionPaymentDTO(x!.UserId, x.Currency!.Code));
        }
    }
}
