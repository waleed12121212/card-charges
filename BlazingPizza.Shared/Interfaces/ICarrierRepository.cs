using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICarrierRepository
{
    Task<List<Carrier>> GetCarriers();
} 