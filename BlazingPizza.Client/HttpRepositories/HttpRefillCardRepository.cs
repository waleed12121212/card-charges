using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

public class HttpRefillCardRepository : IRefillCardRepository
{
    private readonly HttpClient _httpClient;
    public HttpRefillCardRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<List<RefillCard>> GetRefillCardsByCarrier(int carrierId)
    {
        return await _httpClient.GetFromJsonAsync<List<RefillCard>>($"api/refillcards/bycarrier/{carrierId}") ?? new();
    }
} 