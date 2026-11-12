using System.ComponentModel.DataAnnotations;

namespace ClaimBack.API.DTOs
{
    public class UpdatePurchaseDto
    {
        [Required]
        [StringLength(150)]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Retailer { get; set; } = string.Empty;

        public DateTime? ReturnDeadline { get; set; }


        [Required]
        public DateTime PurchaseDate { get; set; }

        [Range(0.01, 999999999)]
        public decimal PurchasePrice { get; set; }

        [Range(1, 120)]
        public int WarrantyDurationMonths { get; set; }
    }
}