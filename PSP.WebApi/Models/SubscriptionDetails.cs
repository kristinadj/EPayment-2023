using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSP.WebApi.Models
{
    [Table("SubscriptionDetails", Schema = "dbo")]
    public class SubscriptionDetails
    {
        [Key]
        public int SubscriptionDetailsId { get; set; }
        public int InvoiceId { get; set; }
        public string? SubscriberName { get; set; }
        public string? SubscriberEmail { get; set; }
        public string? BrandName { get; set; }
        public string? ProductName { get; set; }
        public string? ProductType { get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductCategory { get; set; }

        public Invoice? Invoice { get; set; }
    }
}
