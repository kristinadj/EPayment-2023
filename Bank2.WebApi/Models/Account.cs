using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Bank2.WebApi.Models
{
    [Table("Accounts", Schema = "dbo")]
    public class Account
    {
        public int AccountId { get; set; }

        [Required]
        [StringLength(50)]
        public string AccountNumber { get; set; } = string.Empty;
        public double Balance { get; set; }
        public int CurrencyId { get; set; }
        public string OwnerId { get; set; } = string.Empty;

        public Customer? Owner { get; set; }
        public Currency? Currency { get; set; }
        public ICollection<Card>? Cards { get; set; }
        public ICollection<Transaction>? LocalTransactionsAsIssuer { get; set; }
        public ICollection<Transaction>? LocalTransactionsAsAquirer { get; set; }
        public ICollection<IssuerTransaction>? TransactionsAsIssuer { get; set; }
        public ICollection<AcqurierTransaction>? TransactionsAsAcquirer { get; set; }
    }
}
