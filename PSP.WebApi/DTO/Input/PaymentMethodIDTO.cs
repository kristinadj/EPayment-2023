using System.ComponentModel.DataAnnotations;

namespace PSP.WebApi.DTO.Input
{
    public class PaymentMethodIDTO
    {

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [StringLength(30)]
        public string ServiceName { get; set; }

        [Required]
        [StringLength(30)]
        public string ServiceApiSufix { get; set; }

        public PaymentMethodIDTO(string name, string serviceName, string serviceApiSufix)
        {
            Name = name;
            ServiceName = serviceName;
            ServiceApiSufix = serviceApiSufix;
        }
    }
}
