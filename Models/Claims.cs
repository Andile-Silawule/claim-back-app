
namespace ClaimBack.API.Models
{
    public class Claim
    {
        public int Id { get; set; }

        public string ClaimReference { get; set; } =
            Guid.NewGuid().ToString("N")[..8].ToUpper();

        public int PurchaseId { get; set; }
        public Purchase Purchase { get; set; } = null!;

        public string ClaimType { get; set; } = string.Empty;

        public string ClaimAgainst { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Status { get; set; } = "Submitted";

        public string? Outcome { get; set; }

        public DateTime DateReported { get; set; } = DateTime.UtcNow;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}
