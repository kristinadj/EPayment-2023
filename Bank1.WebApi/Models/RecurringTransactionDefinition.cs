using EntityFrameworkCore.EncryptColumn.Attribute;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank1.WebApi.Models
{
    [Table("RecurringTransactionDefinitions", Schema = "dbo")]
    public class RecurringTransactionDefinition
    {
        [Key]
        public int RecurringTransactionDefinitionId { get; set; }
        public double Amount { get; set; }
        public int CurrencyId { get; set; }

        public string? Description { get; set; }

        [Required]
        public int AquirerAccountId { get; set; }

        [StringLength(100)]
        public string? CardHolderName { get; set; }

        [EncryptColumn]
        public string? PanNumber { get; set; }
        public string? ExpiratoryDate { get; set; }

        [Range(0, 999)]
        public int? CVV { get; set; }
        public int RecurringCycleDays { get; set; }
        public DateTime StartTimestamp { get; set; }
        public DateTime NextPaymentTimestamp { get; set; }
        public bool IsCanceled { get; set; }
        public string? RecurringTransactionSuccessUrl { get; set; }
        public string? RecurringTransactionFailureUrl { get; set; }

        public Currency? Currency { get; set; }
        public Account? AquirerAccount { get; set; }
        public List<RecurringTransaction>? RecurringTransactions { get; set; }
    }
}
