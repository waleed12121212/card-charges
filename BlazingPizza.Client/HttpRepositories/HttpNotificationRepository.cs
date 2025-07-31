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

    public async Task<Notification> CreateNotificationAsync(Notification notification)
    {
        var response = await _httpClient.PostAsJsonAsync("api/notifications", notification);
        return await response.Content.ReadFromJsonAsync<Notification>() ?? new Notification();
    }

    public async Task<Notification?> GetByIdAsync(int id)
    {
        var response = await _httpClient.GetFromJsonAsync<Notification>($"api/notifications/{id}");
        return response;
    }

    public async Task<List<Notification>> GetUserNotificationsAsync(string userId, int limit = 50)
    {
        var response = await _httpClient.GetFromJsonAsync<List<Notification>>($"api/notifications/user/{userId}?limit={limit}");
        return response ?? new List<Notification>();
    }

    public async Task<List<Notification>> GetUnreadNotificationsAsync(string userId)
    {
        var response = await _httpClient.GetFromJsonAsync<List<Notification>>($"api/notifications/user/{userId}/unread");
        return response ?? new List<Notification>();
    }

    public async Task MarkAsReadAsync(int notificationId)
    {
        await _httpClient.PutAsync($"api/notifications/{notificationId}/read", null);
    }

    public async Task MarkAllAsReadAsync(string userId)
    {
        await _httpClient.PutAsync($"api/notifications/user/{userId}/read-all", null);
    }

    public async Task<int> GetUnreadCountAsync(string userId)
    {
        var response = await _httpClient.GetFromJsonAsync<int>($"api/notifications/user/{userId}/unread-count");
        return response;
    }
} 