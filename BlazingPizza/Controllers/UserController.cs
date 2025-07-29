using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlazingPizza.Controllers;

[Route("api/user")]
[ApiController]
[IgnoreAntiforgeryToken]
public class UserController : ControllerBase
{
    [HttpGet("current")]
    public IActionResult GetCurrentUser( )
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(ClaimTypes.Name)?.Value ??
                       User.FindFirst(ClaimTypes.Email)?.Value ??
                       User.Identity.Name ?? "غير محدد";

            Console.WriteLine($"[API] Returning user info: {userId}, {email}");

            return Ok(new
            {
                UserId = userId ,
                Email = email ,
                IsAuthenticated = true
            });
        }

        Console.WriteLine("[API] User not authenticated");
        return Ok(new { IsAuthenticated = false });
    }
}