using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank1.WebApi.Models
{
    [Table("RecurringTransactions", Schema = "dbo")]
    public class RecurringTransaction
    {
        [Key]
        public int RecurringTransactionId { get; set; }

        public int RecurringTransactionDefinitionId { get; set; }
        public int TransactionId { get; set; }

        public RecurringTransactionDefinition? RecurringTransactionDefinition { get; set; }
        public Transaction? Transaction { get; set; }
    }
}
