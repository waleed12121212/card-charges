using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace BlazingPizza.Services;

public class PinService
{
    private readonly PizzaStoreContext _context;
    private const int MAX_PIN_ATTEMPTS = 3;
    private const int LOCKOUT_MINUTES = 15;
    private const int PIN_SESSION_MINUTES = 30;

    public PinService(PizzaStoreContext context)
    {
        _context = context;
    }

    /// <summary>
    /// تشفير الرقم السري باستخدام BCrypt
    /// </summary>
    public string HashPin(string pin)
    {
        if (string.IsNullOrEmpty(pin) || pin.Length != 6 || !pin.All(char.IsDigit))
            throw new ArgumentException("الرقم السري يجب أن يكون 6 أرقام");

        return BCrypt.Net.BCrypt.HashPassword(pin);
    }

    /// <summary>
    /// التحقق من صحة الرقم السري
    /// </summary>
    public bool VerifyPin(string pin , string hash)
    {
        Console.WriteLine($"VerifyPin - PIN: '{pin}', Hash exists: {!string.IsNullOrEmpty(hash)}");

        if (string.IsNullOrEmpty(pin) || string.IsNullOrEmpty(hash))
        {
            Console.WriteLine("PIN or hash is empty");
            return false;
        }

        var result = BCrypt.Net.BCrypt.Verify(pin , hash);
        Console.WriteLine($"BCrypt verification result: {result}");
        return result;
    }

    /// <summary>
    /// تعيين رقم سري جديد للمستخدم
    /// </summary>
    public async Task<bool> SetPinAsync(string userId , string pin)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.PinHash = HashPin(pin);
            user.IsPinRequired = true;
            user.PinAttempts = 0;
            user.PinLockedUntil = null;
            user.PinLastUsed = null;

            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// التحقق من الرقم السري مع معالجة المحاولات الخاطئة
    /// </summary>
    public async Task<PinVerificationResult> VerifyPinAsync(string userId , string pin)
    {
        Console.WriteLine($"PinService.VerifyPinAsync - UserId: {userId}, PIN: '{pin}' (length: {pin?.Length})");

        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            Console.WriteLine("User not found");
            return new PinVerificationResult { Success = false , Message = "المستخدم غير موجود" };
        }

        Console.WriteLine($"User found - PinHash exists: {!string.IsNullOrEmpty(user.PinHash)}");

        // التحقق من وجود رقم سري
        if (string.IsNullOrEmpty(user.PinHash))
        {
            Console.WriteLine("No PIN hash found for user");
            return new PinVerificationResult { Success = false , Message = "لم يتم تعيين رقم سري" };
        }

        // التحقق من القفل
        if (user.PinLockedUntil.HasValue && user.PinLockedUntil.Value > DateTime.UtcNow)
        {
            var remainingMinutes = (int)(user.PinLockedUntil.Value - DateTime.UtcNow).TotalMinutes + 1;
            Console.WriteLine($"User is locked until {user.PinLockedUntil.Value}, remaining minutes: {remainingMinutes}");
            return new PinVerificationResult
            {
                Success = false ,
                IsLocked = true ,
                Message = $"الحساب مقفل لمدة {remainingMinutes} دقيقة"
            };
        }

        Console.WriteLine($"Current PIN attempts: {user.PinAttempts}");

        // التحقق من صحة الرقم السري
        var pinVerificationResult = VerifyPin(pin , user.PinHash);
        Console.WriteLine($"PIN verification result: {pinVerificationResult}");

        if (pinVerificationResult)
        {
            // نجح التحقق - إعادة تعيين المحاولات وتحديث آخر استخدام
            Console.WriteLine("PIN verification successful, resetting attempts");
            user.PinAttempts = 0;
            user.PinLockedUntil = null;
            user.PinLastUsed = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return new PinVerificationResult { Success = true , Message = "تم التحقق بنجاح" };
        }
        else
        {
            Console.WriteLine("PIN verification failed, incrementing attempts");
            user.PinAttempts++;

            if (user.PinAttempts >= MAX_PIN_ATTEMPTS)
            {
                user.PinLockedUntil = DateTime.UtcNow.AddMinutes(LOCKOUT_MINUTES);
                await _context.SaveChangesAsync();

                Console.WriteLine($"User locked due to {user.PinAttempts} failed attempts");
                return new PinVerificationResult
                {
                    Success = false ,
                    IsLocked = true ,
                    Message = $"تم قفل الحساب لمدة {LOCKOUT_MINUTES} دقيقة بسبب المحاولات الخاطئة"
                };
            }
            else
            {
                await _context.SaveChangesAsync();
                int remainingAttempts = MAX_PIN_ATTEMPTS - user.PinAttempts;
                Console.WriteLine($"PIN failed, remaining attempts: {remainingAttempts}");
                return new PinVerificationResult
                {
                    Success = false ,
                    Message = $"رقم سري خاطئ. المحاولات المتبقية: {remainingAttempts}"
                };
            }
        }
    }

    /// <summary>
    /// التحقق من حالة المستخدم للرقم السري
    /// </summary>
    public async Task<PinStatusResult> GetPinStatusAsync(string userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            return new PinStatusResult { HasPin = false };

        return new PinStatusResult
        {
            HasPin = !string.IsNullOrEmpty(user.PinHash) ,
            IsRequired = user.IsPinRequired ,
            IsLocked = user.PinLockedUntil.HasValue && user.PinLockedUntil.Value > DateTime.UtcNow ,
            RemainingAttempts = MAX_PIN_ATTEMPTS - user.PinAttempts ,
            LockoutMinutes = user.PinLockedUntil.HasValue ?
                (int)(user.PinLockedUntil.Value - DateTime.UtcNow).TotalMinutes + 1 : 0
        };
    }

    /// <summary>
    /// حذف الرقم السري
    /// </summary>
    public async Task<bool> RemovePinAsync(string userId)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.PinHash = null;
            user.IsPinRequired = false;
            user.PinAttempts = 0;
            user.PinLockedUntil = null;
            user.PinLastUsed = null;

            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}

public class PinVerificationResult
{
    public bool Success { get; set; }
    public bool IsLocked { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class PinStatusResult
{
    public bool HasPin { get; set; }
    public bool IsRequired { get; set; }
    public bool IsLocked { get; set; }
    public int RemainingAttempts { get; set; }
    public int LockoutMinutes { get; set; }
}