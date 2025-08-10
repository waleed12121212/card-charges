using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BlazingPizza.Services;
using System.Security.Claims;

namespace BlazingPizza.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PinController : ControllerBase
{
    private readonly PinService _pinService;
    private readonly PinSessionService _pinSessionService;

    public PinController(PinService pinService, PinSessionService pinSessionService)
    {
        _pinService = pinService;
        _pinSessionService = pinSessionService;
    }

    [HttpGet("status")]
    public async Task<ActionResult> GetPinStatus()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var status = await _pinService.GetPinStatusAsync(userId);
        return Ok(status);
    }

    [HttpPost("verify")]
    public async Task<ActionResult> VerifyPin([FromBody] VerifyPinRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Console.WriteLine($"PIN verification request - UserId: {userId}, PIN: '{request.Pin}' (length: {request.Pin?.Length})");
        
        if (string.IsNullOrEmpty(userId))
        {
            Console.WriteLine("PIN verification failed: No user ID");
            return Unauthorized();
        }

        var result = await _pinService.VerifyPinAsync(userId, request.Pin);
        Console.WriteLine($"PIN verification result - Success: {result.Success}, Message: {result.Message}");
        
        if (result.Success)
        {
            // Set PIN as verified in session
            await _pinSessionService.SetPinVerifiedAsync(userId);
            Console.WriteLine("PIN session set successfully");
            return Ok(new { success = true });
        }
        else
        {
            Console.WriteLine($"PIN verification failed: {result.Message}");
            return BadRequest(new { success = false, message = result.Message });
        }
    }

    [HttpGet("verify-session")]
    public async Task<ActionResult> VerifySession()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var isVerified = await _pinSessionService.IsPinVerifiedAsync(userId);
        return Ok(isVerified);
    }
}

public class VerifyPinRequest
{
    public string Pin { get; set; } = string.Empty;
} 