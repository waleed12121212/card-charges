using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BlazingPizza.Shared;
using BlazingPizza.Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;

[Route("api/refillcard")]
[ApiController]
[IgnoreAntiforgeryToken]
public class RefillCardController : ControllerBase
{
    private readonly IRefillCardRepository _refillCardRepo;
    private readonly ITransactionRepository _transactionRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RefillCardController(
        IRefillCardRepository refillCardRepo,
        ITransactionRepository transactionRepo,
        IHttpContextAccessor httpContextAccessor)
    {
        _refillCardRepo = refillCardRepo;
        _transactionRepo = transactionRepo;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RefillCard>> GetById(int id)
    {
        var refillCard = await _refillCardRepo.GetByIdAsync(id);
        if (refillCard == null)
            return NotFound();
        return refillCard;
    }

    [HttpGet("carrier/{carrierId}")]
    public async Task<ActionResult<List<RefillCard>>> GetByCarrierId(int carrierId)
    {
        return await _refillCardRepo.GetByCarrierId(carrierId);
    }

    [HttpPost("purchase")]
    [Authorize]
    public async Task<IActionResult> PurchaseRefillCard([FromBody] RefillCardPurchaseRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        // Get the RefillCard details
        var refillCard = await _refillCardRepo.GetByIdAsync(request.RefillCardId);
        if (refillCard == null)
            return NotFound("بطاقة الشحن غير موجودة");

        // Create transaction
        var transaction = new Transaction
        {
            UserId = userId,
            Amount = (decimal)(refillCard.price * request.Quantity),
            Type = "شراء بطاقة شحن",
            Description = $"شراء {request.Quantity} {refillCard.ProductName} - {refillCard.CardAmount} شيكل",
            Date = DateTime.Now,
            Status = TransactionStatus.Completed
        };

        await _transactionRepo.AddAsync(transaction);

        return Ok(new { 
            success = true,
            transactionId = transaction.Id,
            message = "تم شراء البطاقة بنجاح"
        });
    }
}

public class RefillCardPurchaseRequest
{
    public int RefillCardId { get; set; }
    public int Quantity { get; set; } = 1;
} 