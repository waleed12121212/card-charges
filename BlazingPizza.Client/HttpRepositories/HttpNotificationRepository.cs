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

    public async Task<Notification> CreateNotificationAsync(Notification notification)
    {
        var response = await _httpClient.PostAsJsonAsync("api/notifications" , notification);
        return await response.Content.ReadFromJsonAsync<Notification>() ?? new Notification();
    }

    public async Task<Notification?> GetByIdAsync(int id)
    {
        var response = await _httpClient.GetFromJsonAsync<Notification>($"api/notifications/{id}");
        return response;
    }

    public async Task<List<Notification>> GetUserNotificationsAsync(string userId , int limit = 50)
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
        await _httpClient.PutAsync($"api/notifications/{notificationId}/read" , null);
    }

    public async Task MarkAllAsReadAsync(string userId)
    {
        await _httpClient.PutAsync($"api/notifications/user/{userId}/read-all" , null);
    }

    public async Task<int> GetUnreadCountAsync(string userId)
    {
        var response = await _httpClient.GetFromJsonAsync<int>($"api/notifications/user/{userId}/unread-count");
        return response;
    }

    // New method implementations
    public async Task<int> GetUserNotificationsCountAsync(string userId)
    {
        var response = await _httpClient.GetFromJsonAsync<int>($"api/notifications/user/{userId}/count");
        return response;
    }

    public async Task<List<Notification>> GetNotificationsByTypeAsync(string userId, NotificationType type, int limit = 20)
    {
        var response = await _httpClient.GetFromJsonAsync<List<Notification>>($"api/notifications/user/{userId}/type/{type}?limit={limit}");
        return response ?? new List<Notification>();
    }

    public async Task<List<Notification>> GetRecentNotificationsAsync(string userId, int hours = 24)
    {
        var response = await _httpClient.GetFromJsonAsync<List<Notification>>($"api/notifications/user/{userId}/recent?hours={hours}");
        return response ?? new List<Notification>();
    }

    public async Task DeleteOldNotificationsAsync(string userId, int daysOld = 30)
    {
        await _httpClient.DeleteAsync($"api/notifications/user/{userId}/old?daysOld={daysOld}");
    }

    public async Task<Dictionary<NotificationType, int>> GetNotificationSummaryAsync(string userId)
    {
        var response = await _httpClient.GetFromJsonAsync<Dictionary<NotificationType, int>>($"api/notifications/user/{userId}/summary");
        return response ?? new Dictionary<NotificationType, int>();
    }

    public async Task<List<Notification>> SearchNotificationsAsync(string userId, string searchTerm, int limit = 20)
    {
        var response = await _httpClient.GetFromJsonAsync<List<Notification>>($"api/notifications/user/{userId}/search?q={Uri.EscapeDataString(searchTerm)}&limit={limit}");
        return response ?? new List<Notification>();
    }
}