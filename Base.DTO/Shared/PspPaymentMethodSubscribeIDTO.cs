namespace Base.DTO.Shared
{
    public class PspPaymentMethodSubscribeIDTO
    {
        public int MerchantId { get; set; }
        public int PaymentMethodId { get; set; }
        public int Code { get; set; }
        public string Secret { get; set; }

        public PspPaymentMethodSubscribeIDTO(string secret)
        {
            Secret = secret;
        }
    }
}
