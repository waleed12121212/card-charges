using BlazingPizza.Shared;
using BlazingPizza.Shared.Interfaces;
using System.Net.Http.Json;

namespace BlazingPizza.Client.Repositories;

public class HttpInternetPackageRepository : IInternetPackageRepository
{
    private readonly HttpClient _httpClient;

    public HttpInternetPackageRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<InternetPackage>> GetAllAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<List<InternetPackage>>("api/internetpackage");
        return response ?? new List<InternetPackage>();
    }

    public async Task<List<InternetPackage>> GetByCarrierTypeAsync(CarrierType carrierType)
    {
        var response = await _httpClient.GetFromJsonAsync<List<InternetPackage>>($"api/internetpackage/carrier/{(int)carrierType}");
        return response ?? new List<InternetPackage>();
    }

    public async Task<InternetPackage?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<InternetPackage>($"api/internetpackage/{id}");
    }

    public async Task<InternetPackage> CreateAsync(InternetPackage package)
    {
        var response = await _httpClient.PostAsJsonAsync("api/internetpackage", package);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<InternetPackage>();
        return result!;
    }

    public async Task<InternetPackage?> UpdateAsync(InternetPackage package)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/internetpackage/{package.Id}", package);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<InternetPackage>();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/internetpackage/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<List<InternetPackage>> GetActivePackagesAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<List<InternetPackage>>("api/internetpackage/active");
        return response ?? new List<InternetPackage>();
    }

    public async Task<List<InternetPackage>> GetActivePackagesByCarrierAsync(CarrierType carrierType)
    {
        try
        {
            var url = $"api/internetpackage/active/carrier/{(int)carrierType}";
            Console.WriteLine($"Making HTTP request to: {url}");
            
            var response = await _httpClient.GetFromJsonAsync<List<InternetPackage>>(url);
            
            Console.WriteLine($"HTTP response received. Package count: {response?.Count ?? 0}");
            
            return response ?? new List<InternetPackage>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"HTTP request failed: {ex.Message}");
            throw;
        }
    }
} 