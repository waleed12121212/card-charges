using System;
using System.ComponentModel.DataAnnotations;

namespace BlazingPizza.Shared;

public class InternetPackagePurchase
{
    public int Id { get; set; }
    
    [Required]
    public string UserId { get; set; } = string.Empty;
    
    public int InternetPackageId { get; set; }
    
    [Required]
    [Phone]
    [MaxLength(15)]
    public string PhoneNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Carrier type for the phone number
    /// </summary>
    public CarrierType CarrierType { get; set; }
    
    public decimal Amount { get; set; }
    
    public DateTime PurchaseDate { get; set; } = DateTime.Now;
    
    public DateTime ExpiryDate { get; set; }
    
    public InternetPackagePurchaseStatus Status { get; set; } = InternetPackagePurchaseStatus.Pending;
    
    [MaxLength(500)]
    public string? Notes { get; set; }
    
    /// <summary>
    /// Transaction reference from payment gateway
    /// </summary>
    [MaxLength(100)]
    public string? TransactionReference { get; set; }
    
    public int? TransactionId { get; set; }
    
    // Navigation properties
    public InternetPackage? InternetPackage { get; set; }
    public Transaction? Transaction { get; set; }
    
    // Display properties
    public string CarrierDisplayName => CarrierType.GetDisplayName();
}

public enum InternetPackagePurchaseStatus
{
    Pending = 0,
    Completed = 1,
    Failed = 2,
    Cancelled = 3,
    Expired = 4
} 