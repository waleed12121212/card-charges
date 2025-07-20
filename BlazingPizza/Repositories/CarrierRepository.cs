using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CarrierRepository : ICarrierRepository
{
    private readonly PizzaStoreContext _context;
    public CarrierRepository(PizzaStoreContext context)
    {
        _context = context;
    }
    public async Task<List<Carrier>> GetCarriers()
    {
        return await _context.Carriers.ToListAsync();
    }
} 