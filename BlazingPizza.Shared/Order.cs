using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazingPizza.Shared;

public class Order
{
    public int OrderId { get; set; }
    public string? UserId { get; set; }
    public DateTime CreatedTime { get; set; }
    public int CarrierId { get; set; }
    public string? CarrierName { get; set; }
    public List<RefillCardOrder> Cards { get; set; } = new();
    public double TotalPrice => Cards.Sum(c => c.TotalPrice);
}

public class RefillCardOrder
{
    public int Id { get; set; } // مفتاح أساسي
    public int OrderId { get; set; } // مفتاح أجنبي
    [JsonIgnore]
    [NotMapped]
    public Order? Order { get; set; }
    public int RefillCardId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public double UnitPrice { get; set; }
    public double TotalPrice => Quantity * UnitPrice;
}

[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Default , PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(Order))]
[JsonSerializable(typeof(OrderWithStatus))]
[JsonSerializable(typeof(List<OrderWithStatus>))]
[JsonSerializable(typeof(Dictionary<string , string>))]
public partial class OrderContext : JsonSerializerContext { }