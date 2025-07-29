using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazingPizza.Shared;
using BlazingPizza.Shared.Interfaces;

namespace BlazingPizza.Client;

public class HttpRefillCardRepository : IRefillCardRepository
{
    private readonly HttpClient _httpClient;

    public HttpRefillCardRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<RefillCard>> GetByCarrierId(int carrierId)
    {
        return await _httpClient.GetFromJsonAsync<List<RefillCard>>($"api/refillcard/carrier/{carrierId}") ?? new();
    }

    public async Task<RefillCard?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<RefillCard>($"api/refillcard/{id}");
    }

    public async Task<List<RefillCard>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<RefillCard>>("api/refillcard") ?? new();
    }

    public async Task<RefillCard> CreateAsync(RefillCard refillCard)
    {
        var response = await _httpClient.PostAsJsonAsync("api/refillcard", refillCard);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<RefillCard>() ?? refillCard;
    }

    public async Task<RefillCard?> UpdateAsync(RefillCard refillCard)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/refillcard/{refillCard.id}", refillCard);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<RefillCard>();
        }
        return null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/refillcard/{id}");
        return response.IsSuccessStatusCode;
    }
} 