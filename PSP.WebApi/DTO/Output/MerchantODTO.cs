namespace PSP.WebApi.DTO.Output
{
    public class MerchantODTO
    {
        public int MerchantId { get; set; }
        public int MerchantExternalId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ServiceName { get; set; }
        public string TransactionSuccessUrl { get; set; }
        public string TransactionFailureUrl { get; set; }
        public string TransactionErrorUrl { get; set; }

        public MerchantODTO(string name, string address, string phoneNumber, string email, string serviceName, string transactionSuccessUrl, string transactionFailureUrl, string transactionErrorUrl)
        {
            Name = name;
            ServiceName = serviceName;
            Address = address;
            PhoneNumber = phoneNumber;
            Email = email;
            TransactionSuccessUrl = transactionSuccessUrl;
            TransactionFailureUrl = transactionFailureUrl;
            TransactionErrorUrl = transactionErrorUrl;
        }
    }
}
