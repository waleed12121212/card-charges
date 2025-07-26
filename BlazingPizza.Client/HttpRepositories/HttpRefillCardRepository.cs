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
} 