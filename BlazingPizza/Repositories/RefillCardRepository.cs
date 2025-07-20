using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class RefillCardRepository : IRefillCardRepository
{
    private readonly PizzaStoreContext _context;
    public RefillCardRepository(PizzaStoreContext context)
    {
        _context = context;
    }
    public async Task<List<RefillCard>> GetRefillCardsByCarrier(int carrierId)
    {
        return await _context.RefillCards.Where(r => r.CarrierID == carrierId && r.isActive).ToListAsync();
    }
} 