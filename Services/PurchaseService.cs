
using ClaimBack.API.Data;
using ClaimBack.API.DTOs;
using ClaimBack.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ClaimBack.API.Services
{
    public class PurchaseService
    {
        private readonly ClaimBackDbContext _context;

        public PurchaseService(ClaimBackDbContext context)
        {
            _context = context;
        }

        public async Task<List<Purchase>> GetAllPurchasesAsync(string userId)
        {
            return await _context.Purchases
                .Where(p => p.UserId == userId && !p.IsArchived)
                .ToListAsync();
        }

        public async Task<List<Purchase>> GetArchivedPurchasesAsync(string userId)
        {
            return await _context.Purchases
                .Where(p => p.UserId == userId && p.IsArchived)
                .ToListAsync();
        }

        public async Task<Purchase> CreatePurchaseAsync(CreatePurchaseDto dto, string userId)
        {
            var purchase = new Purchase
            {
                ProductName = dto.ProductName,
                Retailer = dto.Retailer,
                PurchaseDate = dto.PurchaseDate,
                PurchasePrice = dto.PurchasePrice,
                WarrantyDurationMonths = dto.WarrantyDurationMonths,
                UserId = userId,
                ReturnDeadline = dto.ReturnDeadline,

            };

            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync();

            return purchase;
        }

        public async Task<Purchase?> GetPurchaseByIdAsync(int id, string userId)
        {
            return await _context.Purchases
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);
        }

        public async Task<Purchase?> UpdatePurchaseAsync(int id, UpdatePurchaseDto dto, string userId)
        {
            var purchase = await _context.Purchases
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (purchase == null)
                return null;

            purchase.ProductName = dto.ProductName;
            purchase.Retailer = dto.Retailer;
            purchase.PurchaseDate = dto.PurchaseDate;
            purchase.PurchasePrice = dto.PurchasePrice;
            purchase.WarrantyDurationMonths = dto.WarrantyDurationMonths;
            purchase.ReturnDeadline = dto.ReturnDeadline;

            await _context.SaveChangesAsync();

            return purchase;
        }

        public async Task<bool> ArchivePurchaseAsync(int id, string userId)
        {
            var purchase = await _context.Purchases
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (purchase == null)
                return false;

            purchase.IsArchived = true;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RestorePurchaseAsync(int id, string userId)
        {
            var purchase = await _context.Purchases
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (purchase == null)
                return false;

            purchase.IsArchived = false;

            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<List<Purchase>> SearchPurchasesAsync(
            PurchaseQueryDto queryDto,
            string userId)
        {
            var query = _context.Purchases
                .Where(p => p.UserId == userId && !p.IsArchived);

            // Search by product name or retailer
            if (!string.IsNullOrWhiteSpace(queryDto.SearchTerm))
            {
                var searchTerm = queryDto.SearchTerm.Trim();

                query = query.Where(p =>
                    p.ProductName.Contains(searchTerm) ||
                    p.Retailer.Contains(searchTerm));
            }

            // Filter by retailer
            if (!string.IsNullOrWhiteSpace(queryDto.Retailer))
            {
                var retailer = queryDto.Retailer.Trim();

                query = query.Where(p =>
                    p.Retailer.Contains(retailer));
            }

            // Sort results
            var sortBy = queryDto.SortBy?.ToLowerInvariant();

            var sortedQuery = sortBy switch
            {
                "productname" => queryDto.SortDescending
                    ? query.OrderByDescending(p => p.ProductName)
                    : query.OrderBy(p => p.ProductName),

                "purchaseprice" => queryDto.SortDescending
                    ? query.OrderByDescending(p => p.PurchasePrice)
                    : query.OrderBy(p => p.PurchasePrice),

                "retailer" => queryDto.SortDescending
                    ? query.OrderByDescending(p => p.Retailer)
                    : query.OrderBy(p => p.Retailer),

                _ => queryDto.SortDescending
                    ? query.OrderByDescending(p => p.PurchaseDate)
                    : query.OrderBy(p => p.PurchaseDate)
            };

            return await sortedQuery.ToListAsync();
        }

    }
}
