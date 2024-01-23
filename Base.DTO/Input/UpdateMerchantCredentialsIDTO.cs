namespace Base.DTO.Input
{
    public class UpdateMerchantCredentialsIDTO
    {
        public int PaymentServiceMerchantId { get; set; }
        public string Code { get; set; }
        public string Secret { get; set; }
        public int? InstitutionId { get; set; }

        public UpdateMerchantCredentialsIDTO(string code, string secret)
        {
            Code = code;
            Secret = secret;
        }
    }
}
