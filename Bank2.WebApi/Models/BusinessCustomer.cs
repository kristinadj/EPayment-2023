using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Bank2.WebApi.Models
{
    [Table("BusinessCustomers", Schema = "dbo")]
    public class BusinessCustomer
    {
        [Key]
        public int BusinessCustomerId { get; set; }
        public int CustomerId { get; set; }

        [Required]
        [StringLength(25)]
        public string Password { get; set; }

        public Customer? Customer { get; set; }

        public BusinessCustomer(string password)
        {
            Password = password;
        }
    }
}
