using Microsoft.Extensions.Primitives;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.WebApi.Models
{
    [Table("Items", Schema = "dbo")]
    public class Item
    {
        [Key]
        public int ItemId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Description { get; set; }
        public double Price { get; set; }
        public int CurrencyId { get; set; }
        public int MerchantId { get; set; }

        public Currency? Currency { get; set; }
        public Merchant? Merchant { get; set; }

        public Item(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
