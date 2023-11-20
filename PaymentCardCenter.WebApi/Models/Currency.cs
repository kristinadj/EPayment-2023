using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PaymentCardCenter.WebApi.Models
{
    [Table("Currencies", Schema = "dbo")]
    public class Currency
    {
        [Key]
        public int CurrencyId { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        [Required]
        [StringLength(4)]
        public string Code { get; set; }

        [Required]
        [StringLength(4)]
        public string Symbol { get; set; }

        public Currency(string name, string code, string symbol)
        {
            Name = name;
            Code = code;
            Symbol = symbol;
        }
    }
}
