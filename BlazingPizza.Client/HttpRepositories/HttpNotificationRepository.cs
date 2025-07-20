using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class HttpNotificationRepository : INotificationRepository
{
    private readonly HttpClient _httpClient;
    public HttpNotificationRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task SubscribeToNotifications(NotificationSubscription subscription)
    {
        await _httpClient.PutAsJsonAsync("api/notifications/subscribe", subscription);
    }
} 