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
    
    public async Task SubscribeToNotifications(NotificationSubscription subscription)
    {
        var existing = await _context.NotificationSubscriptions.FindAsync(subscription.UserId);
        if (existing == null)
        {
            _context.NotificationSubscriptions.Add(subscription);
        }
        else
        {
            _context.Entry(existing).CurrentValues.SetValues(subscription);
        }
        await _context.SaveChangesAsync();
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

    public async Task<List<Notification>> GetUserNotificationsAsync(string userId, int limit = 50)
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
            .CountAsync(n => n.UserId == userId && !n.IsRead);
    }
}