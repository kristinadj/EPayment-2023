using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSP.WebApi.Models
{
    [Table("Merchants", Schema = "dbo")]
    public class Merchant
    {
        public int MerchantId { get; set; }
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

        public ICollection<PaymentMethodMerchant>? PaymentMethods { get; set; }
        public ICollection<Invoice>? Invoices { get; set; }

        public Merchant(string merchantExternalId, string name, string address, string phoneNumber, string email, string serviceName, string transactionSuccessUrl, string transactionFailureUrl, string transactionErrorUrl)
        {
            MerchantExternalId  = merchantExternalId;
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
