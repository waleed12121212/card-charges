using System.Threading.Tasks;
using BlazingPizza.Shared;

public interface INotificationRepository
{
    Task SubscribeToNotifications(NotificationSubscription subscription);
}