
using ClaimBack.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClaimBack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly NotificationService _notificationService;

        public NotificationsController(
            NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        private string UserId =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        // GET: api/Notifications
        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            var notifications =
                await _notificationService.GetUserNotificationsAsync(UserId);

            return Ok(notifications);
        }

        // PATCH: api/Notifications/{id}/read
        [HttpPatch("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var result = await _notificationService.MarkAsReadAsync(
                id,
                UserId);

            if (!result)
                return NotFound(new { message = "Notification not found." });

            return Ok(new { message = "Notification marked as read." });
        }
    }
}
