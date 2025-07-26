using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BlazingPizza.Shared;
using BlazingPizza.Shared.Interfaces;

namespace BlazingPizza;

public class RefillCardRepository : IRefillCardRepository
{
    private readonly PizzaStoreContext _context;

    public RefillCardRepository(PizzaStoreContext context)
    {
        _context = context;
    }

    public async Task<List<RefillCard>> GetByCarrierId(int carrierId)
    {
        return await _context.RefillCards
            .Where(r => r.CarrierID == carrierId)
            .ToListAsync();
    }

    public async Task<RefillCard?> GetByIdAsync(int id)
    {
        return await _context.RefillCards.FindAsync(id);
    }
} 