using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

public class HttpOrderRepository : IOrderRepository
{
    private readonly HttpClient _httpClient;
    public HttpOrderRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<List<OrderWithStatus>> GetOrdersAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<OrderWithStatus>>("api/orders/user-orders") ?? new();
    }
    public async Task<List<OrderWithStatus>> GetOrdersAsync(string userId)
    {
        // تجاهل userId، استخدم الدالة العامة
        return await GetOrdersAsync();
    }
    public async Task<OrderWithStatus> GetOrderWithStatus(int orderId)
    {
        return await _httpClient.GetFromJsonAsync<OrderWithStatus>($"api/orders/{orderId}") ?? new();
    }
    public async Task<OrderWithStatus> GetOrderWithStatus(int orderId, string userId)
    {
        return await _httpClient.GetFromJsonAsync<OrderWithStatus>($"api/orders/{orderId}/user/{userId}") ?? new();
    }
    public async Task<int> PlaceOrder(Order order)
    {
        var response = await _httpClient.PostAsJsonAsync("api/orders", order);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<int>();
        }
        return -1;
    }
    public async Task<List<OrderWithStatus>> GetAllOrdersAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<OrderWithStatus>>("api/orders/all") ?? new();
    }
} 