using Microsoft.AspNetCore.Mvc;
using BlazingPizza.Shared;
using BlazingPizza.Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace BlazingPizza.Controllers;

[Route("api/transaction")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly ITransactionRepository _transactionRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TransactionController(ITransactionRepository transactionRepo, IHttpContextAccessor httpContextAccessor)
    {
        _transactionRepo = transactionRepo;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public async Task<ActionResult<List<Transaction>>> GetAll()
    {
        return await _transactionRepo.GetAllAsync();
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<List<Transaction>>> GetByUserId(string userId)
    {
        return await _transactionRepo.GetByUserIdAsync(userId);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Transaction>> GetById(int id)
    {
        var transaction = await _transactionRepo.GetByIdAsync(id);
        if (transaction == null) return NotFound();
        return transaction;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Add([FromBody] Transaction transaction)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();
        transaction.UserId = userId;
        transaction.Date = DateTime.Now;
        await _transactionRepo.AddAsync(transaction);
        return Ok(new { success = true });
    }
} 