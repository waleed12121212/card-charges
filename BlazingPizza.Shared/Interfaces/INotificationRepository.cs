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
}