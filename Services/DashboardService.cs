
using ClaimBack.API.Data;
using ClaimBack.API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ClaimBack.API.Services
{
    public class DashboardService
    {
        private readonly ClaimBackDbContext _context;

        public DashboardService(ClaimBackDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardStatsDto> GetDashboardStatsAsync(
            string userId)
        {
            var today = DateTime.UtcNow.Date;

            var purchases = _context.Purchases
                .Where(p => p.UserId == userId && !p.IsArchived);

            var claims = _context.Claims
                .Where(c => c.Purchase.UserId == userId);

            return new DashboardStatsDto
            {
                TotalPurchases = await purchases.CountAsync(),

                ActiveWarranties = await purchases.CountAsync(p =>
                    p.PurchaseDate.AddMonths(p.WarrantyDurationMonths) >= today),

                TotalClaims = await claims.CountAsync(),

                ApprovedClaims = await claims.CountAsync(c =>
                    c.Status == "Approved"),

                PendingClaims = await claims.CountAsync(c =>
                    c.Status == "Submitted" ||
                    c.Status == "UnderReview")
            };
        }
    }
}
