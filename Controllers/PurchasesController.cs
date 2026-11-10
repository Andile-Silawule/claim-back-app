
using ClaimBack.API.DTOs;
using ClaimBack.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClaimBack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchasesController : ControllerBase
    {
        private readonly PurchaseService _purchaseService;

        public PurchasesController(PurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPurchases()
        {
            var purchases = await _purchaseService.GetAllPurchasesAsync();

            return Ok(purchases);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPurchaseById(int id)
        {
            var purchase = await _purchaseService.GetPurchaseByIdAsync(id);

            if (purchase is null)
                return NotFound();

            return Ok(purchase);
        }

        [HttpGet("archived")]
        public async Task<IActionResult> GetArchivedPurchases()
        {
            var purchases = await _purchaseService.GetArchivedPurchasesAsync();

            return Ok(purchases);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePurchase(CreatePurchaseDto dto)
        {
            var purchase = await _purchaseService.CreatePurchaseAsync(dto);

            return CreatedAtAction(
                nameof(GetPurchaseById),
                new { id = purchase.Id },
                purchase
            );
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdatePurchase(int id, UpdatePurchaseDto dto)
        {
            var purchase = await _purchaseService.UpdatePurchaseAsync(id, dto);

            if (purchase == null)
                return NotFound();

            return Ok(purchase);
        }

        [HttpPatch("{id:int}/archive")]
        public async Task<IActionResult> ArchivePurchase(int id)
        {
            var archived = await _purchaseService.ArchivePurchaseAsync(id);

            if (!archived)
                return NotFound();

            return NoContent();
        }

        [HttpPatch("{id:int}/restore")]
        public async Task<IActionResult> RestorePurchase(int id)
        {
            var restored = await _purchaseService.RestorePurchaseAsync(id);

            if (!restored)
                return NotFound();

            return NoContent();
        }
    }
}
