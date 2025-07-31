using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BlazingPizza.Shared;
using BlazingPizza.Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;
using BlazingPizza.Services;

[Route("api/refillcard")]
[ApiController]
[IgnoreAntiforgeryToken]
public class RefillCardController : ControllerBase
{
    private readonly IRefillCardRepository _refillCardRepo;
    private readonly ITransactionRepository _transactionRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly NotificationService _notificationService;

    public RefillCardController(
        IRefillCardRepository refillCardRepo,
        ITransactionRepository transactionRepo,
        IHttpContextAccessor httpContextAccessor,
        NotificationService notificationService)
    {
        _refillCardRepo = refillCardRepo;
        _transactionRepo = transactionRepo;
        _httpContextAccessor = httpContextAccessor;
        _notificationService = notificationService;
    }

    [HttpGet]
    public async Task<ActionResult<List<RefillCard>>> GetAll()
    {
        return await _refillCardRepo.GetAllAsync();
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

    [HttpPost]
    public async Task<ActionResult<RefillCard>> Create(RefillCard refillCard)
    {
        if (refillCard == null)
        {
            return BadRequest("RefillCard data is required");
        }

        if (string.IsNullOrWhiteSpace(refillCard.ProductName))
        {
            return BadRequest("Product name is required");
        }

        var createdCard = await _refillCardRepo.CreateAsync(refillCard);
        return CreatedAtAction(nameof(GetById), new { id = createdCard.id }, createdCard);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, RefillCard refillCard)
    {
        if (refillCard == null)
        {
            return BadRequest("RefillCard data is required");
        }

        if (id != refillCard.id)
        {
            return BadRequest("ID mismatch");
        }

        if (string.IsNullOrWhiteSpace(refillCard.ProductName))
        {
            return BadRequest("Product name is required");
        }

        var updatedCard = await _refillCardRepo.UpdateAsync(refillCard);
        if (updatedCard == null)
        {
            return NotFound();
        }

        return Ok(updatedCard);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _refillCardRepo.DeleteAsync(id);
        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPost("purchase")]
    [Authorize]
    public async Task<IActionResult> Purchase([FromBody] RefillCardPurchaseRequest request)
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

        // Create purchase notification
        await _notificationService.CreatePurchaseNotificationAsync(
            userId, 
            $"{request.Quantity} {refillCard.ProductName}", 
            transaction.Amount
        );

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