using BlazingPizza.Shared;
using BlazingPizza.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazingPizza.Repositories;

public class InternetPackagePurchaseRepository : IInternetPackagePurchaseRepository
{
    private readonly PizzaStoreContext _context;

    public InternetPackagePurchaseRepository(PizzaStoreContext context)
    {
        _context = context;
    }

    public async Task<List<InternetPackagePurchase>> GetAllAsync()
    {
        return await _context.InternetPackagePurchases
            .Include(ipp => ipp.InternetPackage)
            .Include(ipp => ipp.Transaction)
            .OrderByDescending(ipp => ipp.PurchaseDate)
            .ToListAsync();
    }

    public async Task<List<InternetPackagePurchase>> GetByUserIdAsync(string userId)
    {
        return await _context.InternetPackagePurchases
            .Include(ipp => ipp.InternetPackage)
            .Include(ipp => ipp.Transaction)
            .Where(ipp => ipp.UserId == userId)
            .OrderByDescending(ipp => ipp.PurchaseDate)
            .ToListAsync();
    }

    public async Task<InternetPackagePurchase?> GetByIdAsync(int id)
    {
        return await _context.InternetPackagePurchases
            .Include(ipp => ipp.InternetPackage)
            .Include(ipp => ipp.Transaction)
            .FirstOrDefaultAsync(ipp => ipp.Id == id);
    }

    public async Task<List<InternetPackagePurchase>> GetByPhoneNumberAsync(string phoneNumber)
    {
        return await _context.InternetPackagePurchases
            .Include(ipp => ipp.InternetPackage)
            .Include(ipp => ipp.Transaction)
            .Where(ipp => ipp.PhoneNumber == phoneNumber)
            .OrderByDescending(ipp => ipp.PurchaseDate)
            .ToListAsync();
    }

    public async Task<List<InternetPackagePurchase>> GetActiveSubscriptionsAsync(string phoneNumber)
    {
        var currentDate = DateTime.Now;
        return await _context.InternetPackagePurchases
            .Include(ipp => ipp.InternetPackage)
            .Include(ipp => ipp.Transaction)
            .Where(ipp => ipp.PhoneNumber == phoneNumber 
                && ipp.Status == InternetPackagePurchaseStatus.Completed
                && ipp.ExpiryDate > currentDate)
            .OrderByDescending(ipp => ipp.PurchaseDate)
            .ToListAsync();
    }

    public async Task<InternetPackagePurchase> CreateAsync(InternetPackagePurchase purchase)
    {
        purchase.PurchaseDate = DateTime.Now;
        _context.InternetPackagePurchases.Add(purchase);
        await _context.SaveChangesAsync();
        return purchase;
    }

    public async Task<InternetPackagePurchase?> UpdateAsync(InternetPackagePurchase purchase)
    {
        var existingPurchase = await _context.InternetPackagePurchases.FindAsync(purchase.Id);
        if (existingPurchase == null) return null;

        existingPurchase.Status = purchase.Status;
        existingPurchase.Notes = purchase.Notes;
        existingPurchase.TransactionReference = purchase.TransactionReference;

        await _context.SaveChangesAsync();
        return existingPurchase;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var purchase = await _context.InternetPackagePurchases.FindAsync(id);
        if (purchase == null) return false;

        _context.InternetPackagePurchases.Remove(purchase);
        await _context.SaveChangesAsync();
        return true;
    }
} 