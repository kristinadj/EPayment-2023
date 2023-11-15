using System.ComponentModel.DataAnnotations;

namespace WebShop.DTO.Output
{
    public class CurrencyODTO
    {
        public int CurrencyId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Symbol { get; set; }

        public CurrencyODTO(string name, string code, string symbol)
        {
            Name = name;
            Code = code;
            Symbol = symbol;
        }
    }
}
