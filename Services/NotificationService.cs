
using ClaimBack.API.Data;
using ClaimBack.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ClaimBack.API.Services
{
    public class NotificationService
    {
        private readonly ClaimBackDbContext _context;

        public NotificationService(ClaimBackDbContext context)
        {
            _context = context;
        }

        // Create a notification
        public async Task<Notification> CreateNotificationAsync(
            string userId,
            string title,
            string message,
            string type)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                Type = type,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Notifications.Add(notification);

            await _context.SaveChangesAsync();

            return notification;
        }

        // Get all notifications for a user
        public async Task<List<Notification>> GetUserNotificationsAsync(
            string userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        // Mark a notification as read
        public async Task<bool> MarkAsReadAsync(
            int notificationId,
            string userId)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n =>
                    n.Id == notificationId &&
                    n.UserId == userId);

            if (notification == null)
                return false;

            notification.IsRead = true;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
