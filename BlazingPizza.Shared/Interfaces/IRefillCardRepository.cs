using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazingPizza.Shared.Interfaces;

public interface IRefillCardRepository
{
    Task<List<RefillCard>> GetByCarrierId(int carrierId);
    Task<RefillCard?> GetByIdAsync(int id);
    Task<List<RefillCard>> GetAllAsync();
    Task<RefillCard> CreateAsync(RefillCard refillCard);
    Task<RefillCard?> UpdateAsync(RefillCard refillCard);
    Task<bool> DeleteAsync(int id);
} 