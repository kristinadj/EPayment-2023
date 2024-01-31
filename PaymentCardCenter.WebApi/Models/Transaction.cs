using PaymentCardCenter.WebApi.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentCardCenter.WebApi.Models
{
    [Table("Transactions", Schema = "dbo")]
    public class Transaction
    {
        public int Transactionid { get; set; }
        public int? AquirerBankId { get; set; }
        public int? AquirerTransctionId { get; set; }
        public DateTime AquirerTimestamp { get; set; }
        public int? IssuerBankId { get; set; }
        public int? IssuerTransactionId { get; set; }
        public DateTime IssuerTimestamp { get; set; }
        public double Amount { get; set; }
        public int CurrencyId { get; set; }

        [Column(TypeName = "nvarchar(24)")]
        public TransactionStatus TransactionStatus { get; set; }

        public Currency? Currency { get; set; }
        public Bank? AquirerBank { get; set; }
        public Bank? IssuerBank { get; set; }
        public ICollection<TransactionLog>? TransactionLogs { get; set; }
    }
}
