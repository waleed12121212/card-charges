using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/refillcards")]
[ApiController]
public class RefillCardsController : ControllerBase
{
    private readonly IRefillCardRepository _repo;
    public RefillCardsController(IRefillCardRepository repo)
    {
        _repo = repo;
    }

    [HttpGet("bycarrier/{carrierId}")]
    public async Task<List<RefillCard>> GetByCarrier(int carrierId)
    {
        return await _repo.GetRefillCardsByCarrier(carrierId);
    }
}