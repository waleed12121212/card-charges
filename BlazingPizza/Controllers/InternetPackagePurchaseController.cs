using BlazingPizza.Shared;
using BlazingPizza.Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BlazingPizza.Services;

namespace BlazingPizza.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InternetPackagePurchaseController : ControllerBase
{
    private readonly IInternetPackagePurchaseRepository _repository;
    private readonly IInternetPackageRepository _packageRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly NotificationService _notificationService;

    public InternetPackagePurchaseController(
        IInternetPackagePurchaseRepository repository,
        IInternetPackageRepository packageRepository,
        ITransactionRepository transactionRepository,
        NotificationService notificationService)
    {
        _repository = repository;
        _packageRepository = packageRepository;
        _transactionRepository = transactionRepository;
        _notificationService = notificationService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<InternetPackagePurchase>>> GetAll()
    {
        var purchases = await _repository.GetAllAsync();
        return Ok(purchases);
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<List<InternetPackagePurchase>>> GetByUserId(string userId)
    {
        // Users can only access their own purchases unless they're admin
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var isAdmin = User.IsInRole("Admin");
        
        if (!isAdmin && currentUserId != userId)
        {
            return Forbid();
        }

        var purchases = await _repository.GetByUserIdAsync(userId);
        return Ok(purchases);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<InternetPackagePurchase>> GetById(int id)
    {
        var purchase = await _repository.GetByIdAsync(id);
        if (purchase == null)
        {
            return NotFound();
        }

        // Users can only access their own purchases unless they're admin
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var isAdmin = User.IsInRole("Admin");
        
        if (!isAdmin && purchase.UserId != currentUserId)
        {
            return Forbid();
        }

        return Ok(purchase);
    }

    [HttpGet("phone/{phoneNumber}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<InternetPackagePurchase>>> GetByPhoneNumber(string phoneNumber)
    {
        var purchases = await _repository.GetByPhoneNumberAsync(phoneNumber);
        return Ok(purchases);
    }

    [HttpGet("active/{phoneNumber}")]
    public async Task<ActionResult<List<InternetPackagePurchase>>> GetActiveSubscriptions(string phoneNumber)
    {
        var purchases = await _repository.GetActiveSubscriptionsAsync(phoneNumber);
        return Ok(purchases);
    }

    [HttpPost]
    public async Task<ActionResult<InternetPackagePurchase>> Create([FromBody] InternetPackagePurchaseRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(currentUserId))
        {
            return Unauthorized();
        }

        // Get the package details
        var package = await _packageRepository.GetByIdAsync(request.InternetPackageId);
        if (package == null)
        {
            return BadRequest("Invalid package ID");
        }

        if (!package.IsActive)
        {
            return BadRequest("Package is not available");
        }

        // Determine carrier type from phone number prefix
        var carrierType = DetermineCarrierTypeFromPhoneNumber(request.PhoneNumber);
        if (carrierType == null)
        {
            return BadRequest("Invalid phone number or unsupported carrier");
        }

        // Verify that the package is for the same carrier as the phone number
        if (package.CarrierType != carrierType.Value)
        {
            return BadRequest("Package carrier type does not match phone number carrier");
        }

        // Create transaction record
        var transaction = new Transaction
        {
            UserId = currentUserId,
            Amount = package.Price,
            Type = "Internet Package",
            Description = $"Internet Package: {package.Name} for {request.PhoneNumber}",
            Date = DateTime.Now,
            Status = TransactionStatus.Completed
        };

        await _transactionRepository.AddAsync(transaction);

        // Create purchase record
        var purchase = new InternetPackagePurchase
        {
            UserId = currentUserId,
            InternetPackageId = request.InternetPackageId,
            PhoneNumber = request.PhoneNumber,
            Amount = package.Price,
            PurchaseDate = DateTime.Now,
            ExpiryDate = DateTime.Now.AddDays(package.ValidityDays),
            Status = InternetPackagePurchaseStatus.Completed,
            TransactionId = transaction.Id,
            Notes = request.Notes
        };

        var createdPurchase = await _repository.CreateAsync(purchase);

        // Create package purchase notification
        await _notificationService.CreatePackagePurchaseNotificationAsync(
            currentUserId,
            package.Name,
            package.Price,
            request.PhoneNumber
        );

        return CreatedAtAction(nameof(GetById), new { id = createdPurchase.Id }, createdPurchase);
    }

    private static CarrierType? DetermineCarrierTypeFromPhoneNumber(string phoneNumber)
    {
        // Remove any spaces or special characters
        var cleanNumber = phoneNumber.Replace(" ", "").Replace("-", "");
        
        return cleanNumber switch
        {
            var num when num.StartsWith("059") => CarrierType.جوال,
            var num when num.StartsWith("056") => CarrierType.أوريدو,
            var num when num.StartsWith("052") => CarrierType.سيليكوم,
            _ => null
        };
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<InternetPackagePurchase>> Update(int id, [FromBody] InternetPackagePurchase purchase)
    {
        if (id != purchase.Id)
        {
            return BadRequest("ID mismatch");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var updatedPurchase = await _repository.UpdateAsync(purchase);
        if (updatedPurchase == null)
        {
            return NotFound();
        }

        return Ok(updatedPurchase);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _repository.DeleteAsync(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}

public class InternetPackagePurchaseRequest
{
    public int InternetPackageId { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Notes { get; set; }
} 