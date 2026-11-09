
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

        public async Task<List<Purchase>> GetAllPurchasesAsync()
        {
            return await _context.Purchases
                .Where(p => !p.IsArchived)
                .ToListAsync();
        }

        public async Task<Purchase> CreatePurchaseAsync(CreatePurchaseDto dto)
        {
            var purchase = new Purchase
            {
                ProductName = dto.ProductName,
                Retailer = dto.Retailer,
                PurchaseDate = dto.PurchaseDate,
                PurchasePrice = dto.PurchasePrice,
                WarrantyDurationMonths = dto.WarrantyDurationMonths
            };

            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync();

            return purchase;
        }

        public async Task<Purchase?> GetPurchaseByIdAsync(int id)
        {
            return await _context.Purchases.FindAsync(id);
        }

        public async Task<Purchase?> UpdatePurchaseAsync(int id, UpdatePurchaseDto dto)
        {
            var purchase = await _context.Purchases.FindAsync(id);

            if (purchase == null)
                return null;

            purchase.ProductName = dto.ProductName;
            purchase.Retailer = dto.Retailer;
            purchase.PurchaseDate = dto.PurchaseDate;
            purchase.PurchasePrice = dto.PurchasePrice;
            purchase.WarrantyDurationMonths = dto.WarrantyDurationMonths;

            await _context.SaveChangesAsync();

            return purchase;
        }

        public async Task<bool> ArchivePurchaseAsync(int id)
        {
            var purchase = await _context.Purchases.FindAsync(id);

            if (purchase == null)
                return false;

            purchase.IsArchived = true;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RestorePurchaseAsync(int id)
        {
            var purchase = await _context.Purchases.FindAsync(id);

            if (purchase == null)
                return false;

            purchase.IsArchived = false;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Purchase>> GetArchivedPurchasesAsync()
        {
            return await _context.Purchases
                .Where(p => p.IsArchived)
                .ToListAsync();
        }
    }
}
