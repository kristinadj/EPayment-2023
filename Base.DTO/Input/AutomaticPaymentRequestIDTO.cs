namespace Base.DTO.Input
{
    public class AutomaticPaymentRequestIDTO : PaymentRequestIDTO
    {
        public ProductIDTO? Product { get; set; }
        public SubscriberIDTO? Subscriber { get; set; }
        public string? BrandName { get; set; }
    }
}
