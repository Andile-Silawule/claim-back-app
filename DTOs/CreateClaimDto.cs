
using System.ComponentModel.DataAnnotations;

namespace ClaimBack.API.DTOs
{
    public class CreateClaimDto
    {
        [Required]
        public int PurchaseId { get; set; }

        [Required]
        [StringLength(30)]
        public string ClaimType { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string ClaimAgainst { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;
    }
}
