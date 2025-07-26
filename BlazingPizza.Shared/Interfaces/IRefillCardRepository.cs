using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazingPizza.Shared.Interfaces;

public interface IRefillCardRepository
{
    Task<List<RefillCard>> GetByCarrierId(int carrierId);
    Task<RefillCard?> GetByIdAsync(int id);
} 