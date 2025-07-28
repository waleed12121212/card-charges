namespace BlazingPizza.Shared;

public class Carrier
{
    public int id { get; set; }
    public string carrierName { get; set; } = string.Empty;
    public bool isActive { get; set; }
    public bool? UnderMaintenance { get; set; }
    public double wholeSalePercent { get; set; }
    public string internetSubscribEmail { get; set; } = string.Empty;
    public string ImeiUserName { get; set; } = string.Empty;
    public string ImeiAPIKey { get; set; } = string.Empty;
    public string ImeiAPIURL { get; set; } = string.Empty;
    public string APIPassword { get; set; } = string.Empty;
    public bool AdsSendSMS { get; set; }
    public bool AdsSendWP { get; set; }
    public int? InvoiceCategory { get; set; }
    public string imageName { get; set; } = string.Empty;

    // Properties for UI display
    public string Name => carrierName;
    public string ImageUrl => $"img/carrier/{imageName}";
}