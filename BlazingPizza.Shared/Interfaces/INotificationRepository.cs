using System.Threading.Tasks;
using BlazingPizza.Shared;

public interface INotificationRepository
{
    Task<Notification> CreateNotificationAsync(Notification notification);
    Task<Notification?> GetByIdAsync(int id);
    Task<List<Notification>> GetUserNotificationsAsync(string userId , int limit = 50);
    Task<List<Notification>> GetUnreadNotificationsAsync(string userId);
    Task MarkAsReadAsync(int notificationId);
    Task MarkAllAsReadAsync(string userId);
    Task<int> GetUnreadCountAsync(string userId);
    
    // New method signatures to match the repository implementation
    Task<int> GetUserNotificationsCountAsync(string userId);
    Task<List<Notification>> GetNotificationsByTypeAsync(string userId, NotificationType type, int limit = 20);
    Task<List<Notification>> GetRecentNotificationsAsync(string userId, int hours = 24);
    Task DeleteOldNotificationsAsync(string userId, int daysOld = 30);
    Task<Dictionary<NotificationType, int>> GetNotificationSummaryAsync(string userId);
    Task<List<Notification>> SearchNotificationsAsync(string userId, string searchTerm, int limit = 20);
}