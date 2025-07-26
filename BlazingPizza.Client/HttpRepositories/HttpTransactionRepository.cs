using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazingPizza.Shared;
using BlazingPizza.Shared.Interfaces;

public class HttpTransactionRepository : ITransactionRepository
{
    private readonly HttpClient _httpClient;
    public HttpTransactionRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Transaction>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Transaction>>("api/transaction") ?? new();
    }

    public async Task<List<Transaction>> GetByUserIdAsync(string userId)
    {
        return await _httpClient.GetFromJsonAsync<List<Transaction>>($"api/transaction/user/{userId}") ?? new();
    }

    public async Task<Transaction?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Transaction>($"api/transaction/{id}");
    }

    public async Task AddAsync(Transaction transaction)
    {
        var response = await _httpClient.PostAsJsonAsync("api/transaction", transaction);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to add transaction: {error}");
        }
    }
} 