using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BlazingPizza.Shared;
using BlazingPizza.Shared.Interfaces;

[Route("api/recharge")]
[ApiController]
public class RechargeController : ControllerBase
{
    private readonly IRechargeRepository _rechargeRepo;
    private readonly ITransactionRepository _transactionRepo;

    public RechargeController(IRechargeRepository rechargeRepo, ITransactionRepository transactionRepo)
    {
        _rechargeRepo = rechargeRepo;
        _transactionRepo = transactionRepo;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRecharge(Recharge recharge)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Create transaction first
        var transaction = new Transaction
        {
            UserId = recharge.UserId,
            Amount = recharge.Amount,
            Type = "شحن رصيد",
            Description = $"شحن رصيد للرقم {recharge.PhoneNumber}",
            Date = DateTime.Now
        };

        await _transactionRepo.AddAsync(transaction);

        // Update recharge with transaction reference
        recharge.TransactionId = transaction.Id;
        recharge.Date = DateTime.Now;

        await _rechargeRepo.AddAsync(recharge);

        return Ok(recharge);
    }
} 