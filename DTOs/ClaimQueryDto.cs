
namespace ClaimBack.API.DTOs
{
    public class ClaimQueryDto
    {
        public string? SearchTerm { get; set; }

        public string? Status { get; set; }

        public string? ClaimType { get; set; }

        public string? SortBy { get; set; }

        public bool SortDescending { get; set; } = true;
    }
}
