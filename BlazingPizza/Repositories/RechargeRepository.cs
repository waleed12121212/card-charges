using BlazingPizza.Shared;
using BlazingPizza.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class RechargeRepository : IRechargeRepository
{
    private readonly PizzaStoreContext _context;
    public RechargeRepository(PizzaStoreContext context)
    {
        _context = context;
    }
    public async Task<List<Recharge>> GetAllAsync()
    {
        return await _context.Recharges.ToListAsync();
    }
    public async Task<List<Recharge>> GetByUserIdAsync(string userId)
    {
        return await _context.Recharges.Where(r => r.UserId == userId).ToListAsync();
    }
    public async Task<Recharge?> GetByIdAsync(int id)
    {
        return await _context.Recharges.FindAsync(id);
    }
    public async Task AddAsync(Recharge recharge)
    {
        _context.Recharges.Add(recharge);
        await _context.SaveChangesAsync();
    }
} 