using System.ComponentModel.DataAnnotations;

namespace Base.DTO.Output
{
    public class InstitutionODTO
    {
        [Required]
        public int? InstitutionId { get; set; }
        public string InstitutionName { get; set; } = string.Empty;
    }
}
