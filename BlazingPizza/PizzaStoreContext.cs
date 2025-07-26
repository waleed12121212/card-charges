using BlazingPizza.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BlazingPizza;

public class PizzaStoreContext : DbContext
{
    public PizzaStoreContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<CarrierStoreUser> Users { get; set; }
    public DbSet<Carrier> Carriers { get; set; }
    public DbSet<RefillCard> RefillCards { get; set; }
    public DbSet<NotificationSubscription> NotificationSubscriptions { get; set; }
    public DbSet<Recharge> Recharges { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RefillCard>()
            .HasOne<Carrier>()
            .WithMany()
            .HasForeignKey(r => r.CarrierID);

        // علاقة Recharge مع Transaction
        modelBuilder.Entity<Recharge>()
            .HasOne(r => r.Transaction)
            .WithMany()
            .HasForeignKey(r => r.TransactionId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
