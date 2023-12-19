namespace Base.DTO.Shared
{
    public class MerchantDTO
    {
        public int MerchantId { get; set; }
        public string MerchantExternalId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ServiceName { get; set; }
        public string TransactionSuccessUrl { get; set; }
        public string TransactionFailureUrl { get; set; }
        public string TransactionErrorUrl { get; set; }

        public MerchantDTO(string merchantExternalId, string name, string address, string phoneNumber, string email, string serviceName)
        {
            MerchantExternalId = merchantExternalId;
            Name = name;
            Address = address;
            PhoneNumber = phoneNumber;
            Email = email;
            ServiceName = serviceName;
            TransactionSuccessUrl = string.Empty;
            TransactionFailureUrl = string.Empty;
            TransactionErrorUrl = string.Empty;
        }
    }
}
