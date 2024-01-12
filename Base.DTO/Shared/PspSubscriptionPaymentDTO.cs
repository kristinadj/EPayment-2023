using Base.DTO.Input;

namespace Base.DTO.Shared
{
    public class PspSubscriptionPaymentDTO : PspInvoiceIDTO
    {
        public ProductIDTO? Product { get; set; }
        public SubscriberIDTO? Subscriber { get; set; }
        public string? BrandName { get; set; }
        public string? RecurringTransactionSuccessUrl { get; set; }
        public string? RecurringTransactionFailureUrl { get; set; }

        public PspSubscriptionPaymentDTO(string issuedToUser, string currencyCode) : base(issuedToUser, currencyCode) { }
        public PspSubscriptionPaymentDTO() : base() { }
    }
}
