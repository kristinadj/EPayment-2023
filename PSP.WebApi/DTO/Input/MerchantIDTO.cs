using System.ComponentModel.DataAnnotations;

namespace PSP.WebApi.DTO.Input
{
    public class MerchantIDTO
    {
        public int MerchantExternalId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

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

        public MerchantIDTO(string name, string serviceName, string transactionSuccessUrl, string transactionFailureUrl, string transactionErrorUrl)
        {
            Name = name;
            ServiceName = serviceName;
            TransactionSuccessUrl = transactionSuccessUrl;
            TransactionFailureUrl = transactionFailureUrl;
            TransactionErrorUrl = transactionErrorUrl;
        }
    }
}
