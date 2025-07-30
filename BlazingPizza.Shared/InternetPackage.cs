using System;
using System.ComponentModel.DataAnnotations;

namespace BlazingPizza.Shared;

public class InternetPackage
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Data amount in MB
    /// </summary>
    public int DataAmountMB { get; set; }
    
    /// <summary>
    /// Validity period in days
    /// </summary>
    public int ValidityDays { get; set; }
    
    /// <summary>
    /// Price in local currency
    /// </summary>
    public decimal Price { get; set; }
    
    /// <summary>
    /// Cost for the provider
    /// </summary>
    public decimal Cost { get; set; }
    
    /// <summary>
    /// Carrier/Network provider type
    /// </summary>
    public CarrierType CarrierType { get; set; }
    
    /// <summary>
    /// API Product ID for integration
    /// </summary>
    public int? ApiProductId { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    
    public DateTime? UpdatedOn { get; set; }
    
    /// <summary>
    /// Sort order for display
    /// </summary>
    public int SortOrder { get; set; }
    
    /// <summary>
    /// Package type (e.g., "Daily", "Weekly", "Monthly")
    /// </summary>
    [MaxLength(50)]
    public string PackageType { get; set; } = string.Empty;
    
    // Display properties
    public string CarrierDisplayName => CarrierType.GetDisplayName();
    
    public string CarrierImageName => CarrierType.GetImageName();
    
    public string DataAmountDisplay => DataAmountMB >= 1024 
        ? $"{DataAmountMB / 1024:F1} GB" 
        : $"{DataAmountMB} MB";
        
    public string ValidityDisplay => ValidityDays == 1 
        ? "يوم واحد" 
        : ValidityDays < 30 
            ? $"{ValidityDays} أيام" 
            : ValidityDays == 30 
                ? "شهر واحد" 
                : $"{ValidityDays / 30} أشهر";
} 