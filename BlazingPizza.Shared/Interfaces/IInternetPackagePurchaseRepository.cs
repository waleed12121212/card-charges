using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazingPizza.Shared.Interfaces;

public interface IInternetPackagePurchaseRepository
{
    Task<List<InternetPackagePurchase>> GetAllAsync();
    Task<List<InternetPackagePurchase>> GetByUserIdAsync(string userId);
    Task<InternetPackagePurchase?> GetByIdAsync(int id);
    Task<InternetPackagePurchase> CreateAsync(InternetPackagePurchase purchase);
    Task<InternetPackagePurchase?> UpdateAsync(InternetPackagePurchase purchase);
    Task<bool> DeleteAsync(int id);
    Task<List<InternetPackagePurchase>> GetByPhoneNumberAsync(string phoneNumber);
    Task<List<InternetPackagePurchase>> GetActiveSubscriptionsAsync(string phoneNumber);
} 