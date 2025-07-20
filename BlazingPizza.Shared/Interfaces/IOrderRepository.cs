using System.Collections.Generic;
using System.Threading.Tasks;
using BlazingPizza.Shared;

public interface IOrderRepository
{
    Task<List<OrderWithStatus>> GetOrdersAsync();
    Task<List<OrderWithStatus>> GetOrdersAsync(string userId);
    Task<OrderWithStatus> GetOrderWithStatus(int orderId);
    Task<OrderWithStatus> GetOrderWithStatus(int orderId , string userId);
    Task<int> PlaceOrder(Order order);
    Task<List<OrderWithStatus>> GetAllOrdersAsync();
}