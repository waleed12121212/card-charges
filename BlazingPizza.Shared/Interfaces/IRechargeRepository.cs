using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazingPizza.Shared.Interfaces;

public interface IRechargeRepository
{
    Task<List<Recharge>> GetAllAsync();
    Task<List<Recharge>> GetByUserIdAsync(string userId);
    Task<Recharge?> GetByIdAsync(int id);
    Task AddAsync(Recharge recharge);
} 