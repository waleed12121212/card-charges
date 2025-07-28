using BlazingPizza.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICarrierRepository
{
    Task<List<Carrier>> GetCarriers( );
    Task<Carrier?> GetCarrierById(int id);
    Task<Carrier> CreateCarrier(Carrier carrier);
    Task<Carrier?> UpdateCarrier(Carrier carrier);
    Task<bool> DeleteCarrier(int id);
}