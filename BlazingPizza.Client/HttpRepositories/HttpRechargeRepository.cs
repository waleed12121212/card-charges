using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazingPizza.Shared;
using BlazingPizza.Shared.Interfaces;

public class HttpRechargeRepository : IRechargeRepository
{
    private readonly HttpClient _httpClient;
    public HttpRechargeRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<List<Recharge>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Recharge>>("api/recharge") ?? new();
    }
    public async Task<List<Recharge>> GetByUserIdAsync(string userId)
    {
        return await _httpClient.GetFromJsonAsync<List<Recharge>>($"api/recharge/user/{userId}") ?? new();
    }
    public async Task<Recharge?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Recharge>($"api/recharge/{id}");
    }
    public async Task AddAsync(Recharge recharge)
    {
        await _httpClient.PostAsJsonAsync("api/recharge", recharge);
    }
} 