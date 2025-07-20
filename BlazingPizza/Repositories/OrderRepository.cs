using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

public class OrderRepository : IOrderRepository
{
    private readonly PizzaStoreContext _context;
    public OrderRepository(PizzaStoreContext context)
    {
        _context = context;
    }
    public async Task<List<OrderWithStatus>> GetOrdersAsync()
    {
        var orders = await _context.Orders
            .Include(o => o.Cards)
            .OrderByDescending(o => o.CreatedTime)
            .ToListAsync();
        return orders.Select(o => OrderWithStatus.FromOrder(o)).ToList();
    }
    public async Task<List<OrderWithStatus>> GetOrdersAsync(string userId)
    {
        var orders = await _context.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.Cards)
            .OrderByDescending(o => o.CreatedTime)
            .ToListAsync();
        return orders.Select(o => OrderWithStatus.FromOrder(o)).ToList();
    }
    public async Task<List<OrderWithStatus>> GetAllOrdersAsync()
    {
        var orders = await _context.Orders
            .Include(o => o.Cards)
            .OrderByDescending(o => o.CreatedTime)
            .ToListAsync();
        return orders.Select(o => OrderWithStatus.FromOrder(o)).ToList();
    }
    public async Task<OrderWithStatus> GetOrderWithStatus(int orderId)
    {
        var order = await _context.Orders
            .Include(o => o.Cards)
            .Where(o => o.OrderId == orderId)
            .SingleOrDefaultAsync();
        if (order is null) throw new System.ArgumentNullException(nameof(order));
        return OrderWithStatus.FromOrder(order);
    }
    public async Task<OrderWithStatus> GetOrderWithStatus(int orderId, string userId)
    {
        var order = await _context.Orders
            .Where(o => o.OrderId == orderId && o.UserId == userId)
            .Include(o => o.Cards)
            .SingleOrDefaultAsync();
        if (order is null) return null;
        return OrderWithStatus.FromOrder(order);
    }
    public async Task<int> PlaceOrder(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order.OrderId;
    }
} 