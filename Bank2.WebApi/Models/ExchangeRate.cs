using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Bank2.WebApi.Models
{
    [Table("ExchangeRates", Schema = "dbo")]
    public class ExchangeRate
    {
        [Key]
        public int ExchangeRateId { get; set; }
        public int FromCurrencyId { get; set; }
        public int ToCurrencyId { get; set; }
        public double Rate { get; set; }

        public Currency? FromCurrency { get; set; }
        public Currency? ToCurrency { get; set; }
    }
}
