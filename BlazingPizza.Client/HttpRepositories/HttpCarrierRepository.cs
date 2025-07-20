using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

public class HttpCarrierRepository : ICarrierRepository
{
    private readonly HttpClient _httpClient;
    public HttpCarrierRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<List<Carrier>> GetCarriers()
    {
        return await _httpClient.GetFromJsonAsync<List<Carrier>>("api/carriers") ?? new();
    }
} 