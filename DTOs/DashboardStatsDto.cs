namespace ClaimBack.API.DTOs
{
    public class DashboardStatsDto
    {
        public int TotalPurchases { get; set; }

        public int ActiveWarranties { get; set; }

        public int TotalClaims { get; set; }

        public int ApprovedClaims { get; set; }

        public int PendingClaims { get; set; }
    }
}