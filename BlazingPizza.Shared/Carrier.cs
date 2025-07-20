public class Carrier
{
    public int id { get; set; }
    public string carrierName { get; set; }
    public bool isActive { get; set; }
    public bool? UnderMaintenance { get; set; }
    public double wholeSalePercent { get; set; }
    public string internetSubscribEmail { get; set; }
    public string ImeiUserName { get; set; }
    public string ImeiAPIKey { get; set; }
    public string ImeiAPIURL { get; set; }
    public string APIPassword { get; set; }
    public bool AdsSendSMS { get; set; }
    public bool AdsSendWP { get; set; }
    public int? InvoiceCategory { get; set; }
} 