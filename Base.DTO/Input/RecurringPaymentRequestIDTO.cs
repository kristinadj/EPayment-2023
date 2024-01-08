namespace Base.DTO.Input
{
    public class RecurringPaymentRequestIDTO : PaymentRequestIDTO
    {
        public ProductIDTO? Product { get; set; }
        public SubscriberIDTO? Subscriber { get; set; }
        public string? BrandName { get; set; }
        public string? PaymentUrl { get; set; }
    }
}
