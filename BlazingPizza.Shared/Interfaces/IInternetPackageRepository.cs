using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazingPizza.Shared.Interfaces;

public interface IInternetPackageRepository
{
    Task<List<InternetPackage>> GetAllAsync();
    Task<List<InternetPackage>> GetByCarrierTypeAsync(CarrierType carrierType);
    Task<InternetPackage?> GetByIdAsync(int id);
    Task<InternetPackage> CreateAsync(InternetPackage package);
    Task<InternetPackage?> UpdateAsync(InternetPackage package);
    Task<bool> DeleteAsync(int id);
    Task<List<InternetPackage>> GetActivePackagesAsync();
    Task<List<InternetPackage>> GetActivePackagesByCarrierAsync(CarrierType carrierType);
} 