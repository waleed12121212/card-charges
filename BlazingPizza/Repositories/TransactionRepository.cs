using BlazingPizza.Shared;
using BlazingPizza.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class TransactionRepository : ITransactionRepository
{
    private readonly PizzaStoreContext _context;
    public TransactionRepository(PizzaStoreContext context)
    {
        _context = context;
    }
    public async Task<List<Transaction>> GetAllAsync()
    {
        return await _context.Transactions
            .Include(t => t.Recharge)
            .Include(t => t.RefillCardOrder)
            .ToListAsync();
    }
    public async Task<List<Transaction>> GetByUserIdAsync(string userId)
    {
        return await _context.Transactions
            .Where(t => t.UserId == userId)
            .Include(t => t.Recharge)
            .Include(t => t.RefillCardOrder)
            .ToListAsync();
    }
    public async Task<Transaction?> GetByIdAsync(int id)
    {
        return await _context.Transactions
            .Include(t => t.Recharge)
            .Include(t => t.RefillCardOrder)
            .FirstOrDefaultAsync(t => t.Id == id);
    }
    public async Task AddAsync(Transaction transaction)
    {
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
    }
} 