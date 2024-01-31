using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EntityFrameworkCore.EncryptColumn.Attribute;

namespace Bank2.WebApi.Models
{
    [Table("BusinessCustomers", Schema = "dbo")]
    public class BusinessCustomer
    {
        [Key]
        public int BusinessCustomerId { get; set; }
        public string CustomerId { get; set; } = string.Empty;

        [Required]
        [EncryptColumn]
        public string SecretKey { get; set; } = string.Empty;

        public int DefaultAccountId { get; set; }

        public Customer? Customer { get; set; }
        public Account? DefaultAccount { get; set; }
    }
}
