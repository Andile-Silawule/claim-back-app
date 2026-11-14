
using ClaimBack.API.Data;
using ClaimBack.API.DTOs;
using ClaimBack.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ClaimBack.API.Services
{
    public class ClaimService
    {
        private readonly ClaimBackDbContext _context;

        public ClaimService(ClaimBackDbContext context)
        {
            _context = context;
        }

        public async Task<Claim?> CreateClaimAsync(
            CreateClaimDto dto,
            string userId)
        {
            var purchase = await _context.Purchases
                .FirstOrDefaultAsync(p =>
                    p.Id == dto.PurchaseId &&
                    p.UserId == userId &&
                    !p.IsArchived);

            if (purchase == null)
                return null;

            var claim = new Claim
            {
                PurchaseId = purchase.Id,
                ClaimType = dto.ClaimType,
                ClaimAgainst = dto.ClaimAgainst,
                CompanyName = dto.CompanyName,
                Description = dto.Description,
                Status = "Submitted",
                DateReported = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();

            return claim;
        }

        public async Task<List<Claim>> GetAllClaimsAsync(string userId)
        {
            return await _context.Claims
                .Where(c => c.Purchase.UserId == userId)
                .Include(c => c.Purchase)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<Claim?> GetClaimByIdAsync(
            int id,
            string userId)
        {
            return await _context.Claims
                .Include(c => c.Purchase)
                .FirstOrDefaultAsync(c =>
                    c.Id == id &&
                    c.Purchase.UserId == userId);
        }


        public async Task<Claim?> UpdateClaimStatusAsync(
            int id,
            UpdateClaimStatusDto dto,
            string userId)
        {
            var allowedStatuses = new[]
            {
        "Submitted",
        "UnderReview",
        "Approved",
        "Rejected",
        "Resolved"
    };

            if (!allowedStatuses.Contains(dto.Status))
                throw new ArgumentException("Invalid claim status.");

            var claim = await _context.Claims
                .FirstOrDefaultAsync(c =>
                    c.Id == id &&
                    c.Purchase.UserId == userId);

            if (claim == null)
                return null;

            var oldStatus = claim.Status;

            if (oldStatus == dto.Status)
                return claim;

            claim.Status = dto.Status;
            claim.UpdatedAt = DateTime.UtcNow;

            var activity = new ClaimActivity
            {
                ClaimId = claim.Id,
                OldStatus = oldStatus,
                NewStatus = dto.Status,
                Description = $"Claim status changed from {oldStatus} to {dto.Status}.",
                PerformedByUserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.ClaimActivities.Add(activity);

            await _context.SaveChangesAsync();

            return claim;
        }


        public async Task<List<ClaimActivity>?> GetClaimActivitiesAsync(
            int claimId,
            string userId)
        {
            var claimExists = await _context.Claims
                .AnyAsync(c =>
                    c.Id == claimId &&
                    c.Purchase.UserId == userId);

            if (!claimExists)
                return null;

            return await _context.ClaimActivities
                .Where(a => a.ClaimId == claimId)
                .OrderBy(a => a.CreatedAt)
                .ToListAsync();
        }


        public async Task<List<Claim>> SearchClaimsAsync(
            ClaimQueryDto queryDto,
            string userId)
        {
            var query = _context.Claims
                .Where(c => c.Purchase.UserId == userId);

            // Search by company name or claim reference
            if (!string.IsNullOrWhiteSpace(queryDto.SearchTerm))
            {
                var searchTerm = queryDto.SearchTerm.Trim();

                query = query.Where(c =>
                    c.CompanyName.Contains(searchTerm) ||
                    c.ClaimReference.Contains(searchTerm));
            }

            // Filter by status
            if (!string.IsNullOrWhiteSpace(queryDto.Status))
            {
                query = query.Where(c =>
                    c.Status == queryDto.Status);
            }

            // Filter by claim type
            if (!string.IsNullOrWhiteSpace(queryDto.ClaimType))
            {
                query = query.Where(c =>
                    c.ClaimType == queryDto.ClaimType);
            }

            // Sort results
            var sortBy = queryDto.SortBy?.ToLowerInvariant();

            var sortedQuery = sortBy switch
            {
                "status" => queryDto.SortDescending
                    ? query.OrderByDescending(c => c.Status)
                    : query.OrderBy(c => c.Status),

                "companyname" => queryDto.SortDescending
                    ? query.OrderByDescending(c => c.CompanyName)
                    : query.OrderBy(c => c.CompanyName),

                _ => queryDto.SortDescending
                    ? query.OrderByDescending(c => c.DateReported)
                    : query.OrderBy(c => c.DateReported)
            };

            return await sortedQuery.ToListAsync();
        }




    }
}
