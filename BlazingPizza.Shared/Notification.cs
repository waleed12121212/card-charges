using System;
using System.ComponentModel.DataAnnotations;

namespace BlazingPizza.Shared;

public class Notification
{
    public int Id { get; set; }
    
    [Required]
    public string UserId { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(500)]
    public string Message { get; set; } = string.Empty;
    
    public NotificationType Type { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public bool IsRead { get; set; } = false;
    
    [MaxLength(100)]
    public string? ActionUrl { get; set; }
    
    public int? RelatedEntityId { get; set; }
    
    [MaxLength(50)]
    public string? RelatedEntityType { get; set; }
}

public enum NotificationType
{
    Purchase = 1,
    CreditTopUp = 2,
    PackagePurchase = 3,
    System = 4,
    Warning = 5,
    Success = 6
} 