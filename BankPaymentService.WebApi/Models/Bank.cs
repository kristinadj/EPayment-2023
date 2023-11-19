

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankPaymentService.WebApi.Models
{
    [Table("Banks", Schema = "dbo")]
    public class Bank
    {
        [Key]
        public int BankId { get; set; }
        public int ExternalBankId { get; set; }   

        [Required]
        [StringLength(50)]
        public string BankName { get; set;}

        [Required]
        [StringLength(50)]
        public string RedirectUrl { get; set; }

        public ICollection<Merchant>? Merchants { get; set; }

        public Bank(string bankName, string redirectUrl)
        {
            BankName = bankName;
            RedirectUrl = redirectUrl;
        }
    }
}
