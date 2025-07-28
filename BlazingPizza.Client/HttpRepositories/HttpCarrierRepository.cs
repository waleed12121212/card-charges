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

    public async Task<Carrier?> GetCarrierById(int id)
    {
        return await _httpClient.GetFromJsonAsync<Carrier>($"api/carriers/{id}");
    }

    public async Task<Carrier> CreateCarrier(Carrier carrier)
    {
        var response = await _httpClient.PostAsJsonAsync("api/carriers", carrier);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Carrier>() ?? carrier;
    }

    public async Task<Carrier?> UpdateCarrier(Carrier carrier)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/carriers/{carrier.id}", carrier);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Carrier>();
        }
        return null;
    }

    public async Task<bool> DeleteCarrier(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/carriers/{id}");
        return response.IsSuccessStatusCode;
    }
} 