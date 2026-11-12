
using System.ComponentModel.DataAnnotations;

namespace ClaimBack.API.DTOs
{
    public class UpdateClaimStatusDto
    {
        [Required]
        [StringLength(30)]
        public string Status { get; set; } = string.Empty;
    }
}
