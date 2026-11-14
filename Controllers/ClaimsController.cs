
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
    public class ClaimsController : ControllerBase
    {
        private readonly ClaimService _claimService;
        private readonly NotificationService _notificationService;

        public ClaimsController(
            ClaimService claimService,
            NotificationService notificationService)
        {
            _claimService = claimService;
            _notificationService = notificationService;
        }

        private string UserId =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpPost]
        public async Task<IActionResult> CreateClaim(CreateClaimDto dto)
        {
            var claim = await _claimService.CreateClaimAsync(dto, UserId);

            if (claim == null)
            {
                return NotFound(new
                {
                    Message = "Purchase not found or you do not have access to it."
                });
            }

            return CreatedAtAction(
                nameof(GetClaimById),
                new { id = claim.Id },
                new
                {
                    claim.Id,
                    claim.ClaimReference,
                    claim.PurchaseId,
                    claim.ClaimType,
                    claim.ClaimAgainst,
                    claim.CompanyName,
                    claim.Description,
                    claim.Status,
                    claim.Outcome,
                    claim.DateReported,
                    claim.CreatedAt
                });
        }

        [HttpGet]
        public async Task<IActionResult> GetClaims(
            [FromQuery] ClaimQueryDto queryDto)
        {
            var claims = await _claimService.SearchClaimsAsync(
                queryDto,
                UserId);

            return Ok(claims.Select(c => new
            {
                c.Id,
                c.ClaimReference,
                c.PurchaseId,
                c.ClaimType,
                c.ClaimAgainst,
                c.CompanyName,
                c.Description,
                c.Status,
                c.Outcome,
                c.DateReported,
                c.CreatedAt,
                c.UpdatedAt
            }));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetClaimById(int id)
        {
            var claim = await _claimService.GetClaimByIdAsync(id, UserId);

            if (claim == null)
                return NotFound();

            return Ok(new
            {
                claim.Id,
                claim.ClaimReference,
                claim.PurchaseId,
                PurchaseName = claim.Purchase.ProductName,
                claim.ClaimType,
                claim.ClaimAgainst,
                claim.CompanyName,
                claim.Description,
                claim.Status,
                claim.Outcome,
                claim.DateReported,
                claim.CreatedAt,
                claim.UpdatedAt
            });
        }

        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> UpdateClaimStatus(
            int id,
            UpdateClaimStatusDto dto)
        {
            try
            {
                var existingClaim =
                    await _claimService.GetClaimByIdAsync(id, UserId);

                if (existingClaim == null)
                    return NotFound();

                var oldStatus = existingClaim.Status;

                var claim = await _claimService.UpdateClaimStatusAsync(
                    id,
                    dto,
                    UserId);

                if (claim == null)
                    return NotFound();

                if (oldStatus != claim.Status)
                {
                    await _notificationService.CreateNotificationAsync(
                        UserId,
                        "Claim status updated",
                        $"Your claim {claim.ClaimReference} has changed from {oldStatus} to {claim.Status}.",
                        "ClaimUpdate");
                }

                return Ok(new
                {
                    claim.Id,
                    claim.ClaimReference,
                    claim.Status,
                    claim.UpdatedAt
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{id:int}/activities")]
        public async Task<IActionResult> GetClaimActivities(int id)
        {
            var activities = await _claimService.GetClaimActivitiesAsync(
                id,
                UserId);

            if (activities == null)
                return NotFound();

            return Ok(activities.Select(a => new
            {
                a.Id,
                a.ClaimId,
                a.OldStatus,
                a.NewStatus,
                a.Description,
                a.PerformedByUserId,
                a.CreatedAt
            }));
        }
    }
}
