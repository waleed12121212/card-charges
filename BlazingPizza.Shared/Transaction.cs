using System;

namespace BlazingPizza.Shared;

public class Transaction
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int? OrderId { get; set; }
    public int? RechargeId { get; set; }
    public int? RefillCardOrderId { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; } = string.Empty; // إيداع/سحب/شحن رصيد ...الخ
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Now;

    // Navigation properties
    public Order? Order { get; set; }
    public Recharge? Recharge { get; set; }
    public RefillCardOrder? RefillCardOrder { get; set; }
} 