namespace BlazingPizza;

public class CarrierStoreUser
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string Role { get; set; } = "User";
    
    // PIN Security Fields
    public string? PinHash { get; set; } // مشفر باستخدام BCrypt
    public int PinAttempts { get; set; } = 0; // عدد المحاولات الخاطئة
    public DateTime? PinLockedUntil { get; set; } // وقت انتهاء القفل
    public DateTime? PinLastUsed { get; set; } // آخر استخدام للرقم السري
    public bool IsPinRequired { get; set; } = false; // هل الرقم السري مطلوب
}