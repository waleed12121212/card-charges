using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BlazingPizza.Shared;
using System.Security.Claims;

namespace BlazingPizza.Controllers;

[ApiController]
[Route("api/notifications")]
[Authorize]
public class NotificationController : ControllerBase
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ILogger<NotificationController> _logger;

    public NotificationController(
        INotificationRepository notificationRepository,
        ILogger<NotificationController> logger)
    {
        _notificationRepository = notificationRepository;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<Notification>> Create([FromBody] Notification notification)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        notification.UserId = userId;
        var createdNotification = await _notificationRepository.CreateNotificationAsync(notification);
        return CreatedAtAction(nameof(GetById), new { id = createdNotification.Id }, createdNotification);
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<List<Notification>>> GetUserNotifications(string userId, [FromQuery] int limit = 50)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (currentUserId != userId && !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        var notifications = await _notificationRepository.GetUserNotificationsAsync(userId, limit);
        return Ok(notifications);
    }

    [HttpGet("user/{userId}/unread")]
    public async Task<ActionResult<List<Notification>>> GetUnreadNotifications(string userId)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (currentUserId != userId && !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        var notifications = await _notificationRepository.GetUnreadNotificationsAsync(userId);
        return Ok(notifications);
    }

    [HttpGet("user/{userId}/unread-count")]
    public async Task<ActionResult<int>> GetUnreadCount(string userId)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (currentUserId != userId && !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        var count = await _notificationRepository.GetUnreadCountAsync(userId);
        return Ok(count);
    }

    [HttpGet("user/{userId}/count")]
    public async Task<ActionResult<int>> GetUserNotificationsCount(string userId)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (currentUserId != userId && !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        var count = await _notificationRepository.GetUserNotificationsCountAsync(userId);
        return Ok(count);
    }

    [HttpGet("user/{userId}/type/{type}")]
    public async Task<ActionResult<List<Notification>>> GetNotificationsByType(string userId, NotificationType type, [FromQuery] int limit = 20)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (currentUserId != userId && !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        var notifications = await _notificationRepository.GetNotificationsByTypeAsync(userId, type, limit);
        return Ok(notifications);
    }

    [HttpGet("user/{userId}/recent")]
    public async Task<ActionResult<List<Notification>>> GetRecentNotifications(string userId, [FromQuery] int hours = 24)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (currentUserId != userId && !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        var notifications = await _notificationRepository.GetRecentNotificationsAsync(userId, hours);
        return Ok(notifications);
    }

    [HttpGet("user/{userId}/summary")]
    public async Task<ActionResult<Dictionary<NotificationType, int>>> GetNotificationSummary(string userId)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (currentUserId != userId && !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        var summary = await _notificationRepository.GetNotificationSummaryAsync(userId);
        return Ok(summary);
    }

    [HttpGet("user/{userId}/search")]
    public async Task<ActionResult<List<Notification>>> SearchNotifications(string userId, [FromQuery] string q, [FromQuery] int limit = 20)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (currentUserId != userId && !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        if (string.IsNullOrWhiteSpace(q))
        {
            return BadRequest("Search term cannot be empty");
        }

        var notifications = await _notificationRepository.SearchNotificationsAsync(userId, q, limit);
        return Ok(notifications);
    }

    [HttpDelete("user/{userId}/old")]
    public async Task<IActionResult> DeleteOldNotifications(string userId, [FromQuery] int daysOld = 30)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (currentUserId != userId && !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        await _notificationRepository.DeleteOldNotificationsAsync(userId, daysOld);
        return Ok();
    }

    [HttpPut("{id}/read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        await _notificationRepository.MarkAsReadAsync(id);
        return Ok();
    }

    [HttpPut("user/{userId}/read-all")]
    public async Task<IActionResult> MarkAllAsRead(string userId)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (currentUserId != userId && !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        await _notificationRepository.MarkAllAsReadAsync(userId);
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Notification>> GetById(int id)
    {
        var notification = await _notificationRepository.GetByIdAsync(id);
        if (notification == null)
        {
            return NotFound();
        }
        
        // Users can only access their own notifications unless they're admin
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var isAdmin = User.IsInRole("Admin");
        
        if (!isAdmin && notification.UserId != currentUserId)
        {
            return Forbid();
        }
        
        return Ok(notification);
    }
} 