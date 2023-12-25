using EntityFrameworkCore.EncryptColumn.Attribute;
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
        [StringLength(19)]
        public string PanNumber { get; set; }

        [Required]
        public string ExpiratoryDate { get; set; }

        [Range(0, 999)]
        [EncryptColumn]
        public int CVV { get; set; }

        public Account? Account { get; set; }

        public Card(string cardHolderName, string panNumber, string expiratoryDate)
        {
            CardHolderName = cardHolderName;
            PanNumber = panNumber;
            ExpiratoryDate = expiratoryDate;
        }
    }
}
