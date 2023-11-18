using System.ComponentModel.DataAnnotations.Schema;

namespace Bank1.WebApi.Models
{
    [Table("Accounts", Schema = "dbo")]
    public class Account
    {
        public int AccountId { get; set; }
        public string AccountNumber { get; set; }
        public double Balance { get; set; }
        public int CurrencyId { get; set; }
        public int OwnerId { get; set; }

        public Customer? Owner { get; set; }
        public Currency? Currency { get; set; }
        public ICollection<Card>? Cards { get; set; }
        public ICollection<Transaction>? TransactionsAsSender { get; set; }
        public ICollection<Transaction>? TransactionsAsReceiver { get; set; }

        public Account(string accountNumber)
        {
            AccountNumber = accountNumber;
        }
    }
}
