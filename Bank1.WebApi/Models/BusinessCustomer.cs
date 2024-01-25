using EntityFrameworkCore.EncryptColumn.Attribute;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank1.WebApi.Models
{
    [Table("BusinessCustomers", Schema = "dbo")]
    public class BusinessCustomer
    {
        [Key]
        public int BusinessCustomerId { get; set; }
        public int CustomerId { get; set; }

        [Required]
        [EncryptColumn]
        public string Password { get; set; }

        public int DefaultAccountId { get; set; }

        public Customer? Customer { get; set; }
        public Account? DefaultAccount { get; set; }

        public BusinessCustomer(string password)
        {
            Password = password;
        }
    }
}
