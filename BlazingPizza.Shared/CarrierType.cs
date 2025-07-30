namespace BlazingPizza.Shared;

public enum CarrierType
{
    جوال = 1,
    أوريدو = 2,
    سيليكوم = 3
}

public static class CarrierTypeExtensions
{
    public static string GetDisplayName(this CarrierType carrierType)
    {
        return carrierType switch
        {
            CarrierType.جوال => "جوال",
            CarrierType.أوريدو => "أوريدو", 
            CarrierType.سيليكوم => "سيليكوم",
            _ => "غير معروف"
        };
    }

    public static string GetImageName(this CarrierType carrierType)
    {
        return carrierType switch
        {
            CarrierType.جوال => "Jawwal.png",
            CarrierType.أوريدو => "ooredoo.png",
            CarrierType.سيليكوم => "celecom.png",
            _ => "default.png"
        };
    }

    public static CarrierType? FromCarrierId(int carrierId)
    {
        return carrierId switch
        {
            1 => CarrierType.جوال,
            2 => CarrierType.أوريدو,
            3 => CarrierType.سيليكوم,
            _ => null
        };
    }

    public static int ToCarrierId(this CarrierType carrierType)
    {
        return (int)carrierType;
    }
} 