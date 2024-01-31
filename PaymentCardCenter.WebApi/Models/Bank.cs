using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentCardCenter.WebApi.Models
{
    [Table("Banks", Schema = "dbo")]
    public class Bank
    {
        [Key]
        public int BankId { get; set; }
        public string CardStartNumbers { get; set; }
        public string AccountNumberStartNumbers { get; set; }
        public string RedirectUrl { get; set; }

        public ICollection<Transaction>? AquirerTransactions { get; set; }
        public ICollection<Transaction>? IssuerTransaction { get; set; }

        public Bank(string cardStartNumbers, string accountNumberStartNumbers, string redirectUrl)
        {
            CardStartNumbers = cardStartNumbers;
            AccountNumberStartNumbers = accountNumberStartNumbers;
            RedirectUrl = redirectUrl;
        }
    }
}
