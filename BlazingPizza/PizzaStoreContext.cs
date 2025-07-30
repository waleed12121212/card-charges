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
    public DbSet<InternetPackage> InternetPackages { get; set; }
    public DbSet<InternetPackagePurchase> InternetPackagePurchases { get; set; }

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

        // Configure CarrierType enum to be stored as integer
        modelBuilder.Entity<InternetPackage>()
            .Property(ip => ip.CarrierType)
            .HasConversion<int>();

        modelBuilder.Entity<InternetPackagePurchase>()
            .Property(ipp => ipp.CarrierType)
            .HasConversion<int>();

        // علاقة InternetPackagePurchase مع InternetPackage
        modelBuilder.Entity<InternetPackagePurchase>()
            .HasOne(ipp => ipp.InternetPackage)
            .WithMany()
            .HasForeignKey(ipp => ipp.InternetPackageId)
            .OnDelete(DeleteBehavior.Restrict);

        // علاقة InternetPackagePurchase مع Transaction
        modelBuilder.Entity<InternetPackagePurchase>()
            .HasOne(ipp => ipp.Transaction)
            .WithMany()
            .HasForeignKey(ipp => ipp.TransactionId)
            .OnDelete(DeleteBehavior.SetNull);

        // Configure decimal precision
        modelBuilder.Entity<InternetPackage>()
            .Property(ip => ip.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<InternetPackage>()
            .Property(ip => ip.Cost)
            .HasPrecision(18, 2);

        modelBuilder.Entity<InternetPackagePurchase>()
            .Property(ipp => ipp.Amount)
            .HasPrecision(18, 2);
    }
}
