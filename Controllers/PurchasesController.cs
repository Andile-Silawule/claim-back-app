
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

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPurchaseById(int id)
        {
            var purchase = await _purchaseService.GetPurchaseByIdAsync(id);

            if (purchase is null)
            {
                return NotFound();
            }

            return Ok(purchase);
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
    }
}
