using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazingPizza.Shared.Interfaces;

public interface ITransactionRepository
{
    Task<List<Transaction>> GetAllAsync();
    Task<List<Transaction>> GetByUserIdAsync(string userId);
    Task<Transaction?> GetByIdAsync(int id);
    Task AddAsync(Transaction transaction);
} 