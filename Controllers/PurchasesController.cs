
using ClaimBack.API.DTOs;
using ClaimBack.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClaimBack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PurchasesController : ControllerBase
    {
        private readonly PurchaseService _purchaseService;
        private readonly DocumentService _documentService;

        public PurchasesController(
            PurchaseService purchaseService,
            DocumentService documentService)
        {
            _purchaseService = purchaseService;
            _documentService = documentService;
        }

        private string UserId =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpGet]
        public async Task<IActionResult> GetPurchases()
        {
            var purchases = await _purchaseService.GetAllPurchasesAsync(UserId);
            return Ok(purchases);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPurchaseById(int id)
        {
            var purchase = await _purchaseService.GetPurchaseByIdAsync(id, UserId);

            if (purchase is null) return NotFound();

            return Ok(purchase);
        }

        [HttpGet("archived")]
        public async Task<IActionResult> GetArchivedPurchases()
        {
            var purchases = await _purchaseService.GetArchivedPurchasesAsync(UserId);
            return Ok(purchases);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePurchase(CreatePurchaseDto dto)
        {
            var purchase = await _purchaseService.CreatePurchaseAsync(dto, UserId);

            return CreatedAtAction(
                nameof(GetPurchaseById),
                new { id = purchase.Id },
                purchase);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdatePurchase(int id, UpdatePurchaseDto dto)
        {
            var purchase = await _purchaseService.UpdatePurchaseAsync(id, dto, UserId);

            if (purchase is null) return NotFound();

            return Ok(purchase);
        }

        [HttpPatch("{id:int}/archive")]
        public async Task<IActionResult> ArchivePurchase(int id)
        {
            var archived = await _purchaseService.ArchivePurchaseAsync(id, UserId);

            if (!archived) return NotFound();

            return NoContent();
        }

        [HttpPatch("{id:int}/restore")]
        public async Task<IActionResult> RestorePurchase(int id)
        {
            var restored = await _purchaseService.RestorePurchaseAsync(id, UserId);

            if (!restored) return NotFound();

            return NoContent();
        }

        [HttpPost("{id:int}/documents")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadDocument(int id, IFormFile file)
        {
            if (file == null)
                return BadRequest(new { Message = "Please select a file." });

            try
            {
                var document = await _documentService.UploadDocumentAsync(
                    id,
                    UserId,
                    file);

                if (document == null)
                    return NotFound(new { Message = "Purchase not found." });

                return Ok(new
                {
                    document.Id,
                    document.PurchaseId,
                    document.FileName,
                    document.ContentType,
                    document.FileSize,
                    document.UploadedAt
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{id:int}/documents")]
        public async Task<IActionResult> GetDocuments(int id)
        {
            var documents = await _documentService.GetDocumentsAsync(id, UserId);

            if (documents == null)
                return NotFound();

            return Ok(documents.Select(d => new
            {
                d.Id,
                d.PurchaseId,
                d.FileName,
                d.ContentType,
                d.FileSize,
                d.UploadedAt
            }));
        }

        [HttpGet("documents/{documentId:int}/download")]
        public async Task<IActionResult> DownloadDocument(int documentId)
        {
            var result = await _documentService.GetDocumentForDownloadAsync( documentId, UserId);

            if (result == null)
                return NotFound();

            var (document, filePath) = result.Value;

            return PhysicalFile(
                filePath,
                document.ContentType,
                document.FileName);
        }
    }
}
