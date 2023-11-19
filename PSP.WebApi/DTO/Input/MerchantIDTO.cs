using System.ComponentModel.DataAnnotations;
using System.Net;

namespace PSP.WebApi.DTO.Input
{
    public class MerchantIDTO
    {
        [StringLength(50)]
        public string MerchantExternalId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        [Required]
        [StringLength(30)]
        public string ServiceName { get; set; }

        [Required]
        [StringLength(70)]
        public string TransactionSuccessUrl { get; set; }

        [Required]
        [StringLength(70)]
        public string TransactionFailureUrl { get; set; }

        [Required]
        [StringLength(70)]
        public string TransactionErrorUrl { get; set; }

        public MerchantIDTO(string merchantExternalId, string name, string address, string phoneNumber, string email, string serviceName, string transactionSuccessUrl, string transactionFailureUrl, string transactionErrorUrl)
        {
            MerchantExternalId = merchantExternalId;
            Name = name;
            Address = address;
            PhoneNumber = phoneNumber;
            Email = email;
            ServiceName = serviceName;
            TransactionSuccessUrl = transactionSuccessUrl;
            TransactionFailureUrl = transactionFailureUrl;
            TransactionErrorUrl = transactionErrorUrl;
        }
    }
}
