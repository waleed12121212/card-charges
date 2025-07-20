using System.Text.Json.Serialization;
using BlazingPizza.ComponentsLibrary.Map;

namespace BlazingPizza.Shared;

public class OrderWithStatus
{
    // Set from DB
    public Order Order { get; set; } = null!;
    // Set from Order
    public string StatusText { get; set; } = null!;
    public bool IsDelivered => StatusText == "تم التسليم";

    public static OrderWithStatus FromOrder(Order order)
    {
        // يمكنك تعديل منطق الحالة حسب النظام الجديد
        string statusText = "تم الاستلام";
        return new OrderWithStatus
        {
            Order = order,
            StatusText = statusText
        };
    }
}