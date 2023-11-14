using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.WebApi.Models
{
    [Table("Invoices", Schema = "dbo")]
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }
        public int OrderId { get; set; }
        public int MerchantId { get; set; }
        public double TotalPrice { get; set; }
        public int CurrencyId { get; set; }
        public int TransactionId { get; set; }

        public Order? Order { get; set; }
        public Merchant? Merchant { get; set; }
        public Currency? Currency { get; set; }
        public Transaction? Transaction { get; set; }
    }
}
