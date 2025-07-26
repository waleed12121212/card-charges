using BlazingPizza;
using BlazingPizza.Shared;
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

}