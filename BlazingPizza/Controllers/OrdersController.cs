using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPush;

namespace BlazingPizza.Controllers;

[Route("api/orders")]
[ApiController]
public class OrdersController : Controller
{
    private readonly PizzaStoreContext _db;

    public OrdersController(PizzaStoreContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderWithStatus>>> GetOrders()
    {
        var orders = await _db.Orders
                .Where(o => o.UserId == PizzaApiExtensions.GetUserId(HttpContext))
                .OrderByDescending(o => o.CreatedTime)
                .ToListAsync();

        return orders.Select(o => OrderWithStatus.FromOrder(o)).ToList();
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<OrderWithStatus>>> GetAllOrders()
    {
        var orders = await _db.Orders
            .Include(o => o.Cards)
            .OrderByDescending(o => o.CreatedTime)
            .ToListAsync();
        return orders.Select(o => OrderWithStatus.FromOrder(o)).ToList();
    }

    [HttpGet("user-orders")]
    public async Task<ActionResult<List<OrderWithStatus>>> GetUserOrders()
    {
        var userId = PizzaApiExtensions.GetUserId(HttpContext);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();
        var orders = await _db.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.Cards)
            .OrderByDescending(o => o.CreatedTime)
            .ToListAsync();
        return orders.Select(o => OrderWithStatus.FromOrder(o)).ToList();
    }

    [HttpGet("{orderId}")]
    public async Task<ActionResult<OrderWithStatus>> GetOrderWithStatus(int orderId)
    {
        var order = await _db.Orders
                .Where(o => o.OrderId == orderId)
                .Where(o => o.UserId == PizzaApiExtensions.GetUserId(HttpContext))
                .SingleOrDefaultAsync();

        if (order == null)
        {
            return NotFound();
        }

        return OrderWithStatus.FromOrder(order);
    }

    [HttpPost]
    public async Task<ActionResult<int>> PlaceOrder(Order order)
    {
        order.CreatedTime = DateTime.Now;
        order.UserId = PizzaApiExtensions.GetUserId(HttpContext);

        _db.Orders.Attach(order);
        await _db.SaveChangesAsync();

        // In the background, send push notifications if possible
        var subscription = await _db.NotificationSubscriptions.Where(e => e.UserId == PizzaApiExtensions.GetUserId(HttpContext)).SingleOrDefaultAsync();
        if (subscription != null)
        {
            _ = SendNotificationAsync(order, subscription, "تم استلام طلبك!");
        }

        return order.OrderId;
    }

    private static async Task SendNotificationAsync(Order order, NotificationSubscription subscription, string message)
    {
        // For a real application, generate your own
        var publicKey = "BLC8GOevpcpjQiLkO7JmVClQjycvTCYWm6Cq_a7wJZlstGTVZvwGFFHMYfXt6Njyvgx_GlXJeo5cSiZ1y4JOx1o";
        var privateKey = "OrubzSz3yWACscZXjFQrrtDwCKg-TGFuWhluQ2wLXDo";

        var pushSubscription = new PushSubscription(subscription.Url, subscription.P256dh, subscription.Auth);
        var vapidDetails = new VapidDetails("mailto:<someone@example.com>", publicKey, privateKey);
        var webPushClient = new WebPushClient();
        try
        {
            var payload = JsonSerializer.Serialize(new
            {
                message,
                url = $"myorders/{order.OrderId}",
            });
            await webPushClient.SendNotificationAsync(pushSubscription, payload, vapidDetails);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error sending push notification: " + ex.Message);
        }
    }
}