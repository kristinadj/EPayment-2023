using System.ComponentModel.DataAnnotations;

namespace Base.DTO.Input
{
    public class RecurringPaymentRequestIDTO : PaymentRequestIDTO
    {
        public ProductIDTO? Product { get; set; }
        public SubscriberIDTO? Subscriber { get; set; }
        public string? BrandName { get; set; }
        public string? PaymentUrl { get; set; }
        public string? RecurringTransactionSuccessUrl { get; set; }
        public string? RecurringTransactionFailureUrl { get; set; }
    }
}
