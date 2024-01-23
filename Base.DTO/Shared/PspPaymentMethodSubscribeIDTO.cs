namespace Base.DTO.Shared
{
    public class PspPaymentMethodSubscribeIDTO
    {
        public int MerchantId { get; set; }
        public int PaymentMethodId { get; set; }
        public string Code { get; set; }
        public string Secret { get; set; }

        public int? InstitutionId { get; set; }

        public PspPaymentMethodSubscribeIDTO(string code, string secret)
        {
            Code = code;
            Secret = secret;
        }
    }
}
