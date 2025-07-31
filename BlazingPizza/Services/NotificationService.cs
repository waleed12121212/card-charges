using BlazingPizza.Shared;

namespace BlazingPizza.Services;

public class NotificationService
{
    private readonly INotificationRepository _notificationRepository;

    public NotificationService(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task CreatePurchaseNotificationAsync(string userId, string itemName, decimal amount, string phoneNumber = "")
    {
        var notification = new Notification
        {
            UserId = userId,
            Title = "تم الشراء بنجاح",
            Message = $"تم شراء {itemName} بمبلغ {amount:0.00} شيكل" + (string.IsNullOrEmpty(phoneNumber) ? "" : $" للرقم {phoneNumber}"),
            Type = NotificationType.Purchase,
            CreatedAt = DateTime.Now
        };

        await _notificationRepository.CreateNotificationAsync(notification);
    }

    public async Task CreateCreditTopUpNotificationAsync(string userId, decimal amount, string phoneNumber)
    {
        var notification = new Notification
        {
            UserId = userId,
            Title = "تم شحن الرصيد بنجاح",
            Message = $"تم شحن رصيد بقيمة {amount:0.00} شيكل للرقم {phoneNumber}",
            Type = NotificationType.CreditTopUp,
            CreatedAt = DateTime.Now
        };

        await _notificationRepository.CreateNotificationAsync(notification);
    }

    public async Task CreatePackagePurchaseNotificationAsync(string userId, string packageName, decimal amount, string phoneNumber)
    {
        var notification = new Notification
        {
            UserId = userId,
            Title = "تم شراء الباقة بنجاح",
            Message = $"تم شراء باقة {packageName} بمبلغ {amount:0.00} شيكل للرقم {phoneNumber}",
            Type = NotificationType.PackagePurchase,
            CreatedAt = DateTime.Now
        };

        await _notificationRepository.CreateNotificationAsync(notification);
    }

    public async Task CreateSystemNotificationAsync(string userId, string title, string message)
    {
        var notification = new Notification
        {
            UserId = userId,
            Title = title,
            Message = message,
            Type = NotificationType.System,
            CreatedAt = DateTime.Now
        };

        await _notificationRepository.CreateNotificationAsync(notification);
    }

    public async Task CreateWarningNotificationAsync(string userId, string title, string message)
    {
        var notification = new Notification
        {
            UserId = userId,
            Title = title,
            Message = message,
            Type = NotificationType.Warning,
            CreatedAt = DateTime.Now
        };

        await _notificationRepository.CreateNotificationAsync(notification);
    }

    public async Task CreateSuccessNotificationAsync(string userId, string title, string message)
    {
        var notification = new Notification
        {
            UserId = userId,
            Title = title,
            Message = message,
            Type = NotificationType.Success,
            CreatedAt = DateTime.Now
        };

        await _notificationRepository.CreateNotificationAsync(notification);
    }
} 