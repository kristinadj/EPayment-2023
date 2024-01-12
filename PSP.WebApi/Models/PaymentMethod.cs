using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSP.WebApi.Models
{
    [Table("PaymentMethods", Schema = "dbo")]
    public class PaymentMethod
    {
        public int PaymentMethodId { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [StringLength(30)]
        public string ServiceName { get; set; }

        [Required]
        [StringLength(30)]
        public string ServiceApiSufix { get; set; }
        public bool SupportsAutomaticPayments { get; set; }

        public PaymentMethod(string name, string serviceName, string serviceApiSufix) 
        {
            Name = name;
            ServiceName = serviceName;
            ServiceApiSufix = serviceApiSufix;
        }
    }
}
