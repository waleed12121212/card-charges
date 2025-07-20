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
    }
}
