using BlazingPizza;
using BlazingPizza.Shared;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class NotificationRepository : INotificationRepository
{
    private readonly PizzaStoreContext _context;
    public NotificationRepository(PizzaStoreContext context)
    {
        _context = context;
    }

    public async Task<Notification> CreateNotificationAsync(Notification notification)
    {
        notification.CreatedAt = DateTime.Now;
        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();
        return notification;
    }

    public async Task<Notification?> GetByIdAsync(int id)
    {
        return await _context.Notifications.FindAsync(id);
    }

    public async Task<List<Notification>> GetUserNotificationsAsync(string userId , int limit = 50)
    {
        return await _context.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<List<Notification>> GetUnreadNotificationsAsync(string userId)
    {
        return await _context.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    public async Task MarkAsReadAsync(int notificationId)
    {
        var notification = await _context.Notifications.FindAsync(notificationId);
        if (notification != null)
        {
            notification.IsRead = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task MarkAllAsReadAsync(string userId)
    {
        var notifications = await _context.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .ToListAsync();

        foreach (var notification in notifications)
        {
            notification.IsRead = true;
        }

        await _context.SaveChangesAsync();
    }

    public async Task<int> GetUnreadCountAsync(string userId)
    {
        return await _context.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .CountAsync();
    }

    // New method to get total notifications count
    public async Task<int> GetUserNotificationsCountAsync(string userId)
    {
        return await _context.Notifications
            .Where(n => n.UserId == userId)
            .CountAsync();
    }

    // New method to get notifications by type
    public async Task<List<Notification>> GetNotificationsByTypeAsync(string userId, NotificationType type, int limit = 20)
    {
        return await _context.Notifications
            .Where(n => n.UserId == userId && n.Type == type)
            .OrderByDescending(n => n.CreatedAt)
            .Take(limit)
            .ToListAsync();
    }

    // New method to get recent notifications (last 24 hours)
    public async Task<List<Notification>> GetRecentNotificationsAsync(string userId, int hours = 24)
    {
        var cutoffTime = DateTime.Now.AddHours(-hours);
        return await _context.Notifications
            .Where(n => n.UserId == userId && n.CreatedAt >= cutoffTime)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    // New method to delete old notifications
    public async Task DeleteOldNotificationsAsync(string userId, int daysOld = 30)
    {
        var cutoffDate = DateTime.Now.AddDays(-daysOld);
        var oldNotifications = await _context.Notifications
            .Where(n => n.UserId == userId && n.CreatedAt < cutoffDate)
            .ToListAsync();

        _context.Notifications.RemoveRange(oldNotifications);
        await _context.SaveChangesAsync();
    }

    // New method to get notification summary by type
    public async Task<Dictionary<NotificationType, int>> GetNotificationSummaryAsync(string userId)
    {
        return await _context.Notifications
            .Where(n => n.UserId == userId)
            .GroupBy(n => n.Type)
            .ToDictionaryAsync(g => g.Key, g => g.Count());
    }

    // New method to search notifications
    public async Task<List<Notification>> SearchNotificationsAsync(string userId, string searchTerm, int limit = 20)
    {
        return await _context.Notifications
            .Where(n => n.UserId == userId && 
                       (n.Title.Contains(searchTerm) || n.Message.Contains(searchTerm)))
            .OrderByDescending(n => n.CreatedAt)
            .Take(limit)
            .ToListAsync();
    }
}