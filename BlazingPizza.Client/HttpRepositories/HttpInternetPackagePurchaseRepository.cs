using BlazingPizza.Shared;
using BlazingPizza.Shared.Interfaces;
using System.Net.Http.Json;

namespace BlazingPizza.Client.HttpRepositories;

public class HttpInternetPackagePurchaseRepository : IInternetPackagePurchaseRepository
{
    private readonly HttpClient _httpClient;

    public HttpInternetPackagePurchaseRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<InternetPackagePurchase>> GetAllAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<List<InternetPackagePurchase>>("api/internetpackagepurchases");
        return response ?? new List<InternetPackagePurchase>();
    }

    public async Task<List<InternetPackagePurchase>> GetByUserIdAsync(string userId)
    {
        var response = await _httpClient.GetFromJsonAsync<List<InternetPackagePurchase>>($"api/internetpackagepurchases/user/{userId}");
        return response ?? new List<InternetPackagePurchase>();
    }

    public async Task<InternetPackagePurchase?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<InternetPackagePurchase>($"api/internetpackagepurchases/{id}");
    }

    public async Task<InternetPackagePurchase> CreateAsync(InternetPackagePurchase purchase)
    {
        var response = await _httpClient.PostAsJsonAsync("api/internetpackagepurchases", purchase);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<InternetPackagePurchase>();
        return result!;
    }

    public async Task<InternetPackagePurchase?> UpdateAsync(InternetPackagePurchase purchase)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/internetpackagepurchases/{purchase.Id}", purchase);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<InternetPackagePurchase>();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/internetpackagepurchases/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<List<InternetPackagePurchase>> GetByPhoneNumberAsync(string phoneNumber)
    {
        var response = await _httpClient.GetFromJsonAsync<List<InternetPackagePurchase>>($"api/internetpackagepurchases/phone/{phoneNumber}");
        return response ?? new List<InternetPackagePurchase>();
    }

    public async Task<List<InternetPackagePurchase>> GetActiveSubscriptionsAsync(string phoneNumber)
    {
        var response = await _httpClient.GetFromJsonAsync<List<InternetPackagePurchase>>($"api/internetpackagepurchases/active/{phoneNumber}");
        return response ?? new List<InternetPackagePurchase>();
    }
}