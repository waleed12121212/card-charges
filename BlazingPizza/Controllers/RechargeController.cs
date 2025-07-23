using Microsoft.AspNetCore.Mvc;
using BlazingPizza.Shared;
using BlazingPizza.Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace BlazingPizza.Controllers;

[Route("api/recharge")]
[ApiController]
public class RechargeController : ControllerBase
{
    private readonly IRechargeRepository _rechargeRepo;
    private readonly ITransactionRepository _transactionRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RechargeController(IRechargeRepository rechargeRepo, ITransactionRepository transactionRepo, IHttpContextAccessor httpContextAccessor)
    {
        _rechargeRepo = rechargeRepo;
        _transactionRepo = transactionRepo;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public async Task<ActionResult<List<Recharge>>> GetAll()
    {
        return await _rechargeRepo.GetAllAsync();
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<List<Recharge>>> GetByUserId(string userId)
    {
        return await _rechargeRepo.GetByUserIdAsync(userId);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Recharge>> GetById(int id)
    {
        var recharge = await _rechargeRepo.GetByIdAsync(id);
        if (recharge == null) return NotFound();
        return recharge;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Add([FromBody] Recharge recharge)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();
        recharge.UserId = userId;
        recharge.Date = DateTime.Now;
        await _rechargeRepo.AddAsync(recharge);
        // إضافة حركة مالية
        var transaction = new Transaction
        {
            UserId = userId,
            RechargeId = recharge.Id,
            Amount = recharge.Amount,
            Type = "شحن رصيد",
            Description = $"شحن رصيد {recharge.Operator} بقيمة {recharge.Amount} شيكل",
            Date = DateTime.Now
        };
        await _transactionRepo.AddAsync(transaction);
        recharge.TransactionId = transaction.Id;
        return Ok(new { success = true });
    }
} 