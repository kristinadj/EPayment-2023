using System.ComponentModel.DataAnnotations;

namespace WebShop.DTO
{
    public class CurrencyDTO
    { 
        public int CurrencyId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Symbol { get; set; }

        public CurrencyDTO(string name, string code, string symbol)
        {
            Name = name;
            Code = code;
            Symbol = symbol;
        }
    }
}
