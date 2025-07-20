using System.Collections.Generic;
using System.Threading.Tasks;

public interface IRefillCardRepository
{
    Task<List<RefillCard>> GetRefillCardsByCarrier(int carrierId);
} 