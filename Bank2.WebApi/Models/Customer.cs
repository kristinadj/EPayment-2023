using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Bank2.WebApi.Models
{
    [Table("Customers", Schema = "dbo")]
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(70)]
        public string Address { get; set; }

        [Required]
        [StringLength(30)]
        public string PhoneNumber { get; set; }

        public string Email { get; set; }


        public ICollection<Account>? Accounts { get; set; }

        public Customer(string firstName, string lastName, string address, string phoneNumber, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            PhoneNumber = phoneNumber;
            Email = email;
        }
    }
}
