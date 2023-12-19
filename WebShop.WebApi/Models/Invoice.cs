using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebShop.DTO.Enums;

namespace WebShop.WebApi.Models
{
    [Table("Invoices", Schema = "dbo")]
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }
        public int MerchantId { get; set; }
        public string UserId { get; set; }
        public double TotalPrice { get; set; }
        public int CurrencyId { get; set; }
        public int TransactionId { get; set; }
        public InvoiceType InvoiceType { get; set; }

        public Merchant? Merchant { get; set; }
        public User? User { get; set; }
        public Currency? Currency { get; set; }
        public Transaction? Transaction { get; set; }

        public Invoice(string userId)
        {
            UserId = userId;
        }
    }
}
