using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Text.Json;

namespace BlazingPizza.Services;

public class PinSessionService
{
    private readonly ProtectedSessionStorage? _protectedSessionStorage;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string PIN_SESSION_KEY = "pin_verified_until";
    private const int SESSION_DURATION_MINUTES = 30;

    public PinSessionService(IHttpContextAccessor httpContextAccessor, ProtectedSessionStorage? protectedSessionStorage = null)
    {
        _httpContextAccessor = httpContextAccessor;
        _protectedSessionStorage = protectedSessionStorage;
    }

    /// <summary>
    /// تسجيل نجاح التحقق من الرقم السري في الجلسة
    /// </summary>
    public async Task SetPinVerifiedAsync(string userId)
    {
        var expiryTime = DateTime.UtcNow.AddMinutes(SESSION_DURATION_MINUTES);
        var sessionData = new PinSessionData 
        { 
            UserId = userId, 
            ExpiryTime = expiryTime 
        };

        // Try to use server-side session first (for API controllers)
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext?.Session != null)
        {
            try
            {
                var sessionKey = $"{PIN_SESSION_KEY}_{userId}";
                var jsonData = JsonSerializer.Serialize(sessionData);
                httpContext.Session.SetString(sessionKey, jsonData);
                return;
            }
            catch
            {
                // Fall through to protected storage
            }
        }

        // Fallback to protected session storage (for Blazor components)
        if (_protectedSessionStorage != null)
        {
            await _protectedSessionStorage.SetAsync($"{PIN_SESSION_KEY}_{userId}", sessionData);
        }
    }

    /// <summary>
    /// التحقق من صحة جلسة الرقم السري
    /// </summary>
    public async Task<bool> IsPinVerifiedAsync(string userId)
    {
        try
        {
            // Try server-side session first (for API controllers)
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.Session != null)
            {
                try
                {
                    var sessionKey = $"{PIN_SESSION_KEY}_{userId}";
                    var jsonData = httpContext.Session.GetString(sessionKey);
                    
                    if (!string.IsNullOrEmpty(jsonData))
                    {
                        var sessionData = JsonSerializer.Deserialize<PinSessionData>(jsonData);
                        
                        if (sessionData != null)
                        {
                            // التحقق من انتهاء صلاحية الجلسة
                            if (DateTime.UtcNow > sessionData.ExpiryTime)
                            {
                                await ClearPinSessionAsync(userId);
                                return false;
                            }

                            return sessionData.UserId == userId;
                        }
                    }
                }
                catch
                {
                    // Fall through to protected storage
                }
            }

            // Fallback to protected session storage (for Blazor components)
            if (_protectedSessionStorage != null)
            {
                var result = await _protectedSessionStorage.GetAsync<PinSessionData>($"{PIN_SESSION_KEY}_{userId}");
                
                if (!result.Success || result.Value == null)
                    return false;

                var sessionData = result.Value;
                
                // التحقق من انتهاء صلاحية الجلسة
                if (DateTime.UtcNow > sessionData.ExpiryTime)
                {
                    await ClearPinSessionAsync(userId);
                    return false;
                }

                return sessionData.UserId == userId;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// مسح جلسة الرقم السري
    /// </summary>
    public async Task ClearPinSessionAsync(string userId)
    {
        try
        {
            // Clear from server-side session
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.Session != null)
            {
                try
                {
                    var sessionKey = $"{PIN_SESSION_KEY}_{userId}";
                    httpContext.Session.Remove(sessionKey);
                }
                catch
                {
                    // Ignore errors
                }
            }

            // Clear from protected session storage
            if (_protectedSessionStorage != null)
            {
                await _protectedSessionStorage.DeleteAsync($"{PIN_SESSION_KEY}_{userId}");
            }
        }
        catch
        {
            // Ignore errors when clearing session
        }
    }

    /// <summary>
    /// تمديد جلسة الرقم السري
    /// </summary>
    public async Task ExtendPinSessionAsync(string userId)
    {
        var isVerified = await IsPinVerifiedAsync(userId);
        if (isVerified)
        {
            await SetPinVerifiedAsync(userId);
        }
    }
}

public class PinSessionData
{
    public string UserId { get; set; } = string.Empty;
    public DateTime ExpiryTime { get; set; }
} 