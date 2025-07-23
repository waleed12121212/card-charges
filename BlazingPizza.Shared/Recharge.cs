using System;

namespace BlazingPizza.Shared;

public class Recharge
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int? OrderId { get; set; }
    public int? TransactionId { get; set; }
    public string Operator { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Now;

    // Navigation properties
    public Order? Order { get; set; }
    public Transaction? Transaction { get; set; }
} 