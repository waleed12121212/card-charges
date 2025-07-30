using BlazingPizza.Shared;
using BlazingPizza.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazingPizza.Repositories;

public class InternetPackageRepository : IInternetPackageRepository
{
    private readonly PizzaStoreContext _context;

    public InternetPackageRepository(PizzaStoreContext context)
    {
        _context = context;
    }

    public async Task<List<InternetPackage>> GetAllAsync()
    {
        return await _context.InternetPackages
            .OrderBy(ip => ip.SortOrder)
            .ThenBy(ip => ip.Name)
            .ToListAsync();
    }

    public async Task<List<InternetPackage>> GetByCarrierTypeAsync(CarrierType carrierType)
    {
        return await _context.InternetPackages
            .Where(ip => ip.CarrierType == carrierType)
            .OrderBy(ip => ip.SortOrder)
            .ThenBy(ip => ip.Name)
            .ToListAsync();
    }

    public async Task<InternetPackage?> GetByIdAsync(int id)
    {
        return await _context.InternetPackages
            .FirstOrDefaultAsync(ip => ip.Id == id);
    }

    public async Task<InternetPackage> CreateAsync(InternetPackage package)
    {
        package.CreatedOn = DateTime.Now;
        _context.InternetPackages.Add(package);
        await _context.SaveChangesAsync();
        return package;
    }

    public async Task<InternetPackage?> UpdateAsync(InternetPackage package)
    {
        var existingPackage = await _context.InternetPackages.FindAsync(package.Id);
        if (existingPackage == null) return null;

        existingPackage.Name = package.Name;
        existingPackage.Description = package.Description;
        existingPackage.DataAmountMB = package.DataAmountMB;
        existingPackage.ValidityDays = package.ValidityDays;
        existingPackage.Price = package.Price;
        existingPackage.Cost = package.Cost;
        existingPackage.CarrierType = package.CarrierType;
        existingPackage.ApiProductId = package.ApiProductId;
        existingPackage.IsActive = package.IsActive;
        existingPackage.SortOrder = package.SortOrder;
        existingPackage.PackageType = package.PackageType;
        existingPackage.UpdatedOn = DateTime.Now;

        await _context.SaveChangesAsync();
        return existingPackage;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var package = await _context.InternetPackages.FindAsync(id);
        if (package == null) return false;

        _context.InternetPackages.Remove(package);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<InternetPackage>> GetActivePackagesAsync()
    {
        return await _context.InternetPackages
            .Where(ip => ip.IsActive)
            .OrderBy(ip => ip.SortOrder)
            .ThenBy(ip => ip.Name)
            .ToListAsync();
    }

    public async Task<List<InternetPackage>> GetActivePackagesByCarrierAsync(CarrierType carrierType)
    {
        return await _context.InternetPackages
            .Where(ip => ip.CarrierType == carrierType && ip.IsActive)
            .OrderBy(ip => ip.SortOrder)
            .ThenBy(ip => ip.Name)
            .ToListAsync();
    }
} 