using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BlazingPizza;

public class PizzaStoreContext : DbContext
{
    public PizzaStoreContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<CarrierStoreUser> Users { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<Carrier> Carriers { get; set; }
    public DbSet<RefillCard> RefillCards { get; set; }

    public DbSet<NotificationSubscription> NotificationSubscriptions { get; set; }

    public DbSet<RefillCardOrder> RefillCardOrders { get; set; }

    public DbSet<Recharge> Recharges { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RefillCard>()
            .HasOne<Carrier>()
            .WithMany()
            .HasForeignKey(r => r.CarrierID);

        // تعريف العلاقة بين Order وCards
        modelBuilder.Entity<Order>()
            .HasMany(o => o.Cards)
            .WithOne(c => c.Order)
            .HasForeignKey(c => c.OrderId);

        // علاقة Recharge مع Order وTransaction
        modelBuilder.Entity<Recharge>()
            .HasOne(r => r.Order)
            .WithMany()
            .HasForeignKey(r => r.OrderId)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<Recharge>()
            .HasOne(r => r.Transaction)
            .WithMany()
            .HasForeignKey(r => r.TransactionId)
            .OnDelete(DeleteBehavior.SetNull);

        // علاقة Transaction مع Order وRecharge
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Order)
            .WithMany()
            .HasForeignKey(t => t.OrderId)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Recharge)
            .WithMany()
            .HasForeignKey(t => t.RechargeId)
            .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.RefillCardOrder)
            .WithMany()
            .HasForeignKey(t => t.RefillCardOrderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
