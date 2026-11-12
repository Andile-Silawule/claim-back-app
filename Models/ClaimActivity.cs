
namespace ClaimBack.API.Models
{
    public class ClaimActivity
    {
        public int Id { get; set; }

        public int ClaimId { get; set; }
        public Claim Claim { get; set; } = null!;

        public string? OldStatus { get; set; }

        public string? NewStatus { get; set; }

        public string Description { get; set; } = string.Empty;

        public string? PerformedByUserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
