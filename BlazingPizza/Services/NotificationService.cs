using BlazingPizza.Shared;
using Microsoft.AspNetCore.SignalR;
using BlazingPizza.Hubs;

namespace BlazingPizza.Services;

public class NotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly OneSignalService _oneSignalService;
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly IUserService _userService;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(
        INotificationRepository notificationRepository, 
        OneSignalService oneSignalService,
        IHubContext<NotificationHub> hubContext,
        IUserService userService,
        ILogger<NotificationService> logger)
    {
        _notificationRepository = notificationRepository;
        _oneSignalService = oneSignalService;
        _hubContext = hubContext;
        _userService = userService;
        _logger = logger;
        
        _logger.LogInformation("NotificationService initialized successfully");
    }

    public async Task CreatePurchaseNotificationAsync(string userId, string itemName, decimal amount, string phoneNumber = "")
    {
        _logger.LogInformation($"Creating purchase notification for user {userId}: {itemName}");
        
        var title = "تم الشراء بنجاح";
        var message = $"تم شراء {itemName} بمبلغ {amount:0.00} شيكل" + (string.IsNullOrEmpty(phoneNumber) ? "" : $" للرقم {phoneNumber}");
        
        await CreateAndSendNotificationAsync(userId, title, message, NotificationType.Purchase);
    }

    public async Task CreateCreditTopUpNotificationAsync(string userId, decimal amount, string phoneNumber)
    {
        _logger.LogInformation($"Creating credit top-up notification for user {userId}: {amount} NIS for {phoneNumber}");
        
        var title = "تم شحن الرصيد بنجاح";
        var message = $"تم شحن رصيد بقيمة {amount:0.00} شيكل للرقم {phoneNumber}";
        
        await CreateAndSendNotificationAsync(userId, title, message, NotificationType.CreditTopUp);
    }

    public async Task CreatePackagePurchaseNotificationAsync(string userId, string packageName, decimal amount, string phoneNumber)
    {
        _logger.LogInformation($"Creating package purchase notification for user {userId}: {packageName}");
        
        var title = "تم شراء الباقة بنجاح";
        var message = $"تم شراء باقة {packageName} بمبلغ {amount:0.00} شيكل للرقم {phoneNumber}";
        
        await CreateAndSendNotificationAsync(userId, title, message, NotificationType.PackagePurchase);
    }

    public async Task CreateSystemNotificationAsync(string userId, string title, string message)
    {
        _logger.LogInformation($"Creating system notification for user {userId}: {title}");
        await CreateAndSendNotificationAsync(userId, title, message, NotificationType.System);
    }

    public async Task CreateWarningNotificationAsync(string userId, string title, string message)
    {
        _logger.LogInformation($"Creating warning notification for user {userId}: {title}");
        await CreateAndSendNotificationAsync(userId, title, message, NotificationType.Warning);
    }

    public async Task CreateSuccessNotificationAsync(string userId, string title, string message)
    {
        _logger.LogInformation($"Creating success notification for user {userId}: {title}");
        await CreateAndSendNotificationAsync(userId, title, message, NotificationType.Success);
    }

    private async Task CreateAndSendNotificationAsync(string userId, string title, string message, NotificationType type, string? actionUrl = null)
    {
        _logger.LogInformation($"Starting notification creation for user {userId}. Title: {title}, Type: {type}");
        
        try
        {
            // Step 1: Save to database
            _logger.LogDebug($"Saving notification to database for user {userId}");
            var notification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                Type = type,
                ActionUrl = actionUrl,
                CreatedAt = DateTime.Now
            };

            var savedNotification = await _notificationRepository.CreateNotificationAsync(notification);
            _logger.LogInformation($"Notification saved to database with ID: {savedNotification.Id}");
            
            // Step 2: Send real-time notification via SignalR
            try
            {
                _logger.LogDebug($"Sending SignalR notification to user group: user_{userId}");
                await _hubContext.Clients.Group($"user_{userId}")
                    .SendAsync("ReceiveNotification", new
                    {
                        Id = savedNotification.Id,
                        Title = title,
                        Message = message,
                        Type = type.ToString(),
                        CreatedAt = savedNotification.CreatedAt,
                        ActionUrl = actionUrl
                    });
                _logger.LogInformation($"SignalR notification sent successfully to user {userId}");
            }
            catch (Exception signalREx)
            {
                _logger.LogError(signalREx, $"Failed to send SignalR notification to user {userId}");
                // Continue with OneSignal even if SignalR fails
            }

            // Step 3: Send push notification via OneSignal
            try
            {
                _logger.LogDebug($"Sending OneSignal push notification to user {userId}");
                await _oneSignalService.SendNotificationToUserAsync(userId, title, message, actionUrl);
                _logger.LogInformation($"OneSignal notification sent successfully to user {userId}");
            }
            catch (Exception oneSignalEx)
            {
                _logger.LogError(oneSignalEx, $"Failed to send OneSignal notification to user {userId}");
                // Don't throw - we still want the notification saved in DB
            }
            
            _logger.LogInformation($"Notification process completed successfully for user {userId}: {title}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to create notification for user {userId}: {title}");
            throw; // Re-throw to let caller handle it
        }
    }

    // New method for admin to send broadcast notifications
    public async Task SendBroadcastNotificationAsync(string title, string message, string? actionUrl = null, NotificationType type = NotificationType.System)
    {
        _logger.LogInformation($"Starting broadcast notification: {title} of type {type}");
        
        try
        {
            // Step 1: Create a system notification record for admin tracking
            var adminNotification = new Notification
            {
                UserId = "admin", // Special user ID for admin notifications
                Title = title,
                Message = message,
                Type = type,
                ActionUrl = actionUrl,
                CreatedAt = DateTime.Now
            };

            // Save admin tracking record to database first
            await _notificationRepository.CreateNotificationAsync(adminNotification);
            _logger.LogInformation("Broadcast notification saved to database for admin tracking");
            
            // Step 2: Send real-time notification to all connected users via SignalR
            try
            {
                _logger.LogDebug("Sending SignalR broadcast notification to all connected clients");
                await _hubContext.Clients.All.SendAsync("ReceiveBroadcast", new
                {
                    Title = title,
                    Message = message,
                    Type = type.ToString(),
                    CreatedAt = DateTime.Now,
                    ActionUrl = actionUrl
                });
                _logger.LogInformation("SignalR broadcast notification sent to all connected clients");
            }
            catch (Exception signalREx)
            {
                _logger.LogError(signalREx, "Failed to send SignalR broadcast notification");
            }
            
            // Step 3: Send push notification to all users via OneSignal
            try
            {
                _logger.LogDebug("Sending OneSignal broadcast notification");
                await _oneSignalService.SendNotificationToAllUsersAsync(title, message, actionUrl);
                _logger.LogInformation("OneSignal broadcast notification sent to all users");
            }
            catch (Exception oneSignalEx)
            {
                _logger.LogError(oneSignalEx, "Failed to send OneSignal broadcast notification");
                // Don't throw - SignalR notification was successful
            }
            
            // Step 4: Optionally create individual notification records for all users
            // This is helpful for notification history but can be resource intensive
            try
            {
                _logger.LogDebug("Creating individual notification records for broadcast");
                await CreateIndividualNotificationRecordsAsync(title, message, actionUrl, type);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create individual notification records, but broadcast was sent");
                // Don't throw - the broadcast was successful
            }
            
            _logger.LogInformation($"Broadcast notification completed: {title}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to send broadcast notification: {title}");
            throw;
        }
    }

    // Helper method to create individual notification records for all users
    private async Task CreateIndividualNotificationRecordsAsync(string title, string message, string? actionUrl = null, NotificationType type = NotificationType.System)
    {
        try
        {
            _logger.LogInformation("Creating individual notification records for all active users");
            
            // Get all active user IDs
            var userIds = await _userService.GetAllActiveUserIdsAsync();
            
            if (userIds.Count == 0)
            {
                _logger.LogWarning("No active users found for individual notification records");
                return;
            }

            _logger.LogInformation($"Creating individual notifications for {userIds.Count} users");

            // Create notifications in batches to avoid overwhelming the database
            const int batchSize = 50;
            var totalCreated = 0;

            for (int i = 0; i < userIds.Count; i += batchSize)
            {
                var batch = userIds.Skip(i).Take(batchSize).ToList();
                
                foreach (var userId in batch)
                {
                    try
                    {
                        var notification = new Notification
                        {
                            UserId = userId,
                            Title = title,
                            Message = message,
                            Type = type,
                            ActionUrl = actionUrl,
                            CreatedAt = DateTime.Now
                        };

                        await _notificationRepository.CreateNotificationAsync(notification);
                        totalCreated++;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Failed to create individual notification for user {userId}");
                        // Continue with other users even if one fails
                    }
                }

                // Small delay between batches to avoid overwhelming the database
                if (i + batchSize < userIds.Count)
                {
                    await Task.Delay(100);
                }
            }

            _logger.LogInformation($"Successfully created {totalCreated} individual notification records out of {userIds.Count} users");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create individual notification records");
            throw;
        }
    }

    // New method for admin to send notifications to specific segment
    public async Task SendSegmentNotificationAsync(string segment, string title, string message, string? actionUrl = null)
    {
        _logger.LogInformation($"Starting segment notification to {segment}: {title}");
        
        try
        {
            // Create a system notification record for admin tracking
            var notification = new Notification
            {
                UserId = "admin_segment", // Special user ID for admin segment notifications
                Title = title,
                Message = $"[{segment}] {message}", // Include segment info in message
                Type = NotificationType.System,
                ActionUrl = actionUrl,
                CreatedAt = DateTime.Now
            };

            // Save to database first
            await _notificationRepository.CreateNotificationAsync(notification);
            _logger.LogInformation($"Segment notification saved to database for {segment}");
            
            // Send push notification to segment via OneSignal
            try
            {
                await _oneSignalService.SendNotificationToSegmentAsync(segment, title, message, actionUrl);
                _logger.LogInformation($"OneSignal segment notification sent to {segment}");
            }
            catch (Exception oneSignalEx)
            {
                _logger.LogError(oneSignalEx, $"Failed to send OneSignal segment notification to {segment}");
                throw;
            }
            
            _logger.LogInformation($"Segment notification completed for {segment}: {title}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to send segment notification to {segment}: {title}");
            throw;
        }
    }

    // New method to get notification statistics
    public async Task<NotificationStats> GetNotificationStatsAsync(string userId)
    {
        _logger.LogDebug($"Getting notification stats for user {userId}");
        
        var totalCount = await _notificationRepository.GetUserNotificationsCountAsync(userId);
        var unreadCount = await _notificationRepository.GetUnreadCountAsync(userId);
        
        return new NotificationStats
        {
            TotalCount = totalCount,
            UnreadCount = unreadCount,
            ReadCount = totalCount - unreadCount
        };
    }
}

public class NotificationStats
{
    public int TotalCount { get; set; }
    public int UnreadCount { get; set; }
    public int ReadCount { get; set; }
} 