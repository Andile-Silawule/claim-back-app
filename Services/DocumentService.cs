
using ClaimBack.API.Data;
using ClaimBack.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ClaimBack.API.Services
{
    public class DocumentService
    {
        private readonly ClaimBackDbContext _context;
        private readonly IWebHostEnvironment _environment;

        private const long MaxFileSize = 10 * 1024 * 1024;

        private readonly string[] _allowedExtensions =
        {
            ".pdf", ".jpg", ".jpeg", ".png"
        };

        public DocumentService(
            ClaimBackDbContext context,
            IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<PurchaseDocument?> UploadDocumentAsync(
            int purchaseId,
            string userId,
            IFormFile file)
        {
            var purchase = await _context.Purchases
                .FirstOrDefaultAsync(p =>
                    p.Id == purchaseId && p.UserId == userId);

            if (purchase == null)
                return null;

            if (file.Length == 0 || file.Length > MaxFileSize)
                throw new ArgumentException(
                    "File must be between 1 byte and 10 MB.");

            var extension = Path.GetExtension(file.FileName)
                .ToLowerInvariant();

            if (!_allowedExtensions.Contains(extension))
                throw new ArgumentException(
                    "Only PDF, JPG, JPEG and PNG files are allowed.");

            var uploadDirectory = Path.Combine(
                _environment.ContentRootPath,
                "PrivateUploads");

            Directory.CreateDirectory(uploadDirectory);

            var storedFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(
                uploadDirectory,
                storedFileName);

            await using (var stream = new FileStream(
                filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var document = new PurchaseDocument
            {
                PurchaseId = purchaseId,
                FileName = Path.GetFileName(file.FileName),
                StoredFileName = storedFileName,
                ContentType = file.ContentType,
                FileSize = file.Length,
                UploadedAt = DateTime.UtcNow
            };

            _context.PurchaseDocuments.Add(document);
            await _context.SaveChangesAsync();

            return document;
        }

        public async Task<List<PurchaseDocument>?> GetDocumentsAsync(
            int purchaseId,
            string userId)
        {
            var purchaseExists = await _context.Purchases
                .AnyAsync(p =>
                    p.Id == purchaseId && p.UserId == userId);

            if (!purchaseExists)
                return null;

            return await _context.PurchaseDocuments
                .Where(d => d.PurchaseId == purchaseId)
                .ToListAsync();
        }

        public async Task<(PurchaseDocument Document, string FilePath)?>
            GetDocumentForDownloadAsync(
                int documentId,
                string userId)
        {
            var document = await _context.PurchaseDocuments
                .FirstOrDefaultAsync(d =>
                    d.Id == documentId &&
                    d.Purchase.UserId == userId);

            if (document == null)
                return null;

            var filePath = Path.Combine(
                _environment.ContentRootPath,
                "PrivateUploads",
                document.StoredFileName);

            if (!File.Exists(filePath))
                return null;

            return (document, filePath);
        }
    }
}
