using System;

namespace BlazingPizza.Shared;

public class Recharge
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Operator { get; set; }
    public decimal Amount { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime Date { get; set; }
    public int? TransactionId { get; set; }
    public Transaction? Transaction { get; set; }
} 