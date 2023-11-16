using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSP.WebApi.Models
{
    [Table("InvoiceItems", Schema = "dbo")]
    public class InvoiceItem
    {
        [Key]
        public int InvoiceItemId { get; set; }
        public int InvoiceId { get; set; }
        public int ExternalItemId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public int CurrencyId { get; set; }

        public Invoice? Invoice { get; set; }
        public Currency? Currency { get; set; }
    }
}
