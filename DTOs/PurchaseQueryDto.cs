
namespace ClaimBack.API.DTOs
{
    public class PurchaseQueryDto
    {
        public string? SearchTerm { get; set; }

        public string? Retailer { get; set; }

        public string? SortBy { get; set; }

        public bool SortDescending { get; set; } = true;
    }
}
