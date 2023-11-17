using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.WebApi.Models
{
    [Table("PaymentMethods", Schema = "dbo")]
    public class PaymentMethod
    {
        [Key]
        public int PaymentMethodId { get; set; }

        public int PspPaymentMethodId { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public ICollection<Transaction>? Transactions { get; set; }

        public PaymentMethod(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
