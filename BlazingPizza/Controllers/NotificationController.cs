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

    public NotificationController(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
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