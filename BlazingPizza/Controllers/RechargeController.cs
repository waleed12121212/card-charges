using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BlazingPizza.Shared;
using BlazingPizza.Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

[Route("api/recharge")]
[ApiController]
[IgnoreAntiforgeryToken]
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

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateRecharge(Recharge recharge)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Get the current user ID from authentication
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        // Create transaction first
        var transaction = new Transaction
        {
            UserId = userId, // Use authenticated user ID
            Amount = recharge.Amount,
            Type = "شحن رصيد",
            Description = $"شحن رصيد للرقم {recharge.PhoneNumber}",
            Date = DateTime.Now
        };

        await _transactionRepo.AddAsync(transaction);

        // Update recharge with transaction reference and user ID
        recharge.UserId = userId; // Set authenticated user ID
        recharge.TransactionId = transaction.Id;
        recharge.Date = DateTime.Now;

        await _rechargeRepo.AddAsync(recharge);

        return Ok(recharge);
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
} 