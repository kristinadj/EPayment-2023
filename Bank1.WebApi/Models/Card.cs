using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank1.WebApi.Models
{
    [Table("Cards", Schema = "dbo")]
    public class Card
    {
        [Key]
        public int CardId { get; set; }
        public int AccountId { get; set; }

        [Required]
        [StringLength(100)]
        public string CardHolderName { get; set; }

        [Required]
        [StringLength(16)]
        public string PanNumber { get; set; }

        [Required]
        public string ExpiratoryDate { get; set; }

        [Range(0, 999)]
        public int CVV { get; set; }

        public Account? Account { get; set; }

        public Card(string cardHolderName, string panNumber) 
        {
            CardHolderName = cardHolderName;
            PanNumber = panNumber;
        }
    }
}
