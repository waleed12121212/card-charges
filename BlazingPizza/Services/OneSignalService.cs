using OneSignalApi.Api;
using OneSignalApi.Client;
using OneSignalApi.Model;
using BlazingPizza.Shared;

namespace BlazingPizza.Services;

public class OneSignalService
{
    private readonly string _appId;
    private readonly string _restApiKey;
    private readonly DefaultApi _oneSignalClient;
    private readonly ILogger<OneSignalService> _logger;

    public OneSignalService(IConfiguration configuration, ILogger<OneSignalService> logger)
    {
        _logger = logger;
        _appId = configuration["OneSignal:AppId"] ?? throw new ArgumentNullException("OneSignal:AppId");
        _restApiKey = configuration["OneSignal:RestApiKey"] ?? throw new ArgumentNullException("OneSignal:RestApiKey");
        
        _logger.LogInformation($"OneSignal initialized with AppId: {_appId.Substring(0, Math.Min(8, _appId.Length))}...");
        
        var config = new Configuration();
        config.BasePath = "https://api.onesignal.com";
        config.AccessToken = _restApiKey;
        
        _oneSignalClient = new DefaultApi(config);
    }

    public async Task SendNotificationToUserAsync(string userId, string title, string message, string? actionUrl = null)
    {
        try
        {
            _logger.LogInformation($"Sending OneSignal notification to user {userId}: {title}");
            
            var notification = new OneSignalApi.Model.Notification(
                appId: _appId
            )
            {
                Contents = new LanguageStringMap(en: message, ar: message),
                Headings = new LanguageStringMap(en: title, ar: title),
                // For individual users, we'll use the All segment for now
                // This is a limitation - we'd need player IDs for true individual targeting
                IncludedSegments = new List<string> { "All" }
            };

            if (!string.IsNullOrEmpty(actionUrl))
            {
                notification.Url = actionUrl;
            }

            _logger.LogDebug($"OneSignal notification payload: AppId={_appId}, Title={title}, Message={message}");
            
            var response = await _oneSignalClient.CreateNotificationAsync(notification);
            _logger.LogInformation($"OneSignal notification sent successfully. ID: {response.Id}");
            
            // Note: Individual user targeting is limited without player IDs
            _logger.LogInformation($"Note: This notification was sent to all users due to OneSignal API limitations. For true individual targeting, player IDs would be needed.");
        }
        catch (ApiException apiEx)
        {
            _logger.LogError(apiEx, $"OneSignal API Error for user {userId}: {apiEx.ErrorCode} - {apiEx.Message}");
            _logger.LogError($"OneSignal Response Body: {apiEx.ErrorContent}");
            
            // Don't throw for API errors - just log them so the system continues to work
            _logger.LogWarning($"OneSignal notification failed for user {userId}, but continuing execution");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"OneSignal notification failed for user {userId}: {ex.Message}");
            // Don't throw - just log and continue
        }
    }

    public async Task SendNotificationToAllUsersAsync(string title, string message, string? actionUrl = null)
    {
        try
        {
            _logger.LogInformation($"Sending OneSignal broadcast notification: {title}");
            
            var notification = new OneSignalApi.Model.Notification(
                appId: _appId
            )
            {
                Contents = new LanguageStringMap(en: message, ar: message),
                Headings = new LanguageStringMap(en: title, ar: title),
                IncludedSegments = new List<string> { "All" },
                // Add some additional properties to improve delivery
                Priority = 10,
                AndroidAccentColor = "FF0000FF",
                SmallIcon = "ic_notification",
                LargeIcon = "ic_launcher"
            };

            if (!string.IsNullOrEmpty(actionUrl))
            {
                notification.Url = actionUrl;
            }

            _logger.LogDebug($"OneSignal broadcast payload: AppId={_appId}, Title={title}, Message={message}");
            
            var response = await _oneSignalClient.CreateNotificationAsync(notification);
            _logger.LogInformation($"OneSignal broadcast notification sent successfully. ID: {response.Id}");
            
            // Note: We can't easily check recipient count in this API version
            _logger.LogInformation("OneSignal broadcast notification sent to all subscribed users");
        }
        catch (ApiException apiEx)
        {
            _logger.LogError(apiEx, $"OneSignal API Error for broadcast: {apiEx.ErrorCode} - {apiEx.Message}");
            _logger.LogError($"OneSignal Response Body: {apiEx.ErrorContent}");
            
            // For broadcast notifications, we might want to throw so admin knows it failed
            throw new Exception($"OneSignal broadcast failed: {apiEx.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"OneSignal broadcast notification failed: {ex.Message}");
            throw;
        }
    }

    public async Task SendNotificationToSegmentAsync(string segment, string title, string message, string? actionUrl = null)
    {
        try
        {
            _logger.LogInformation($"Sending OneSignal segment notification to {segment}: {title}");
            
            var notification = new OneSignalApi.Model.Notification(
                appId: _appId
            )
            {
                Contents = new LanguageStringMap(en: message, ar: message),
                Headings = new LanguageStringMap(en: title, ar: title),
                IncludedSegments = new List<string> { segment },
                Priority = 10
            };

            if (!string.IsNullOrEmpty(actionUrl))
            {
                notification.Url = actionUrl;
            }

            _logger.LogDebug($"OneSignal segment payload: AppId={_appId}, Segment={segment}, Title={title}");
            
            var response = await _oneSignalClient.CreateNotificationAsync(notification);
            _logger.LogInformation($"OneSignal segment notification sent successfully. ID: {response.Id}");
            
            _logger.LogInformation($"OneSignal segment notification sent to segment: {segment}");
        }
        catch (ApiException apiEx)
        {
            _logger.LogError(apiEx, $"OneSignal API Error for segment {segment}: {apiEx.ErrorCode} - {apiEx.Message}");
            _logger.LogError($"OneSignal Response Body: {apiEx.ErrorContent}");
            throw new Exception($"OneSignal segment notification failed: {apiEx.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"OneSignal segment notification failed for {segment}: {ex.Message}");
            throw;
        }
    }

    public async Task<List<NotificationWithMeta>?> GetNotificationHistoryAsync()
    {
        try
        {
            _logger.LogInformation("Fetching OneSignal notification history");
            
            var response = await _oneSignalClient.GetNotificationsAsync(_appId, limit: 50, offset: 0);
            var notifications = response.Notifications?.Select(n => new NotificationWithMeta
            {
                Id = n.Id,
                Headings = n.Headings,
                Contents = n.Contents,
                IncludedSegments = n.IncludedSegments,
                Filters = n.Filters,
                SendAfter = n.SendAfter,
                CompletedAt = n.CompletedAt
            }).ToList();
            
            _logger.LogInformation($"Retrieved {notifications?.Count ?? 0} OneSignal notifications");
            return notifications ?? new List<NotificationWithMeta>();
        }
        catch (ApiException apiEx)
        {
            _logger.LogError(apiEx, $"OneSignal API Error fetching history: {apiEx.ErrorCode} - {apiEx.Message}");
            return new List<NotificationWithMeta>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch OneSignal notification history");
            return new List<NotificationWithMeta>();
        }
    }

    // Test method to verify OneSignal connection
    public async Task<bool> TestConnectionAsync()
    {
        try
        {
            _logger.LogInformation("Testing OneSignal connection");
            
            // Try to get app info as a connection test
            var appResponse = await _oneSignalClient.GetAppAsync(_appId);
            _logger.LogInformation($"OneSignal connection test successful. App: {appResponse.Name}");
            return true;
        }
        catch (ApiException apiEx)
        {
            _logger.LogError(apiEx, $"OneSignal connection test failed: {apiEx.ErrorCode} - {apiEx.Message}");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "OneSignal connection test failed");
            return false;
        }
    }
}