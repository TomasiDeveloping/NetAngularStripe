using Microsoft.EntityFrameworkCore;
using NetAngularStripe.Data.Entities;

namespace NetAngularStripe.Data;

public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
{
    public  DbSet<Company> Companies { get; set; }

    public  DbSet<LicenseType> LicenseTypes { get; set; }

    public  DbSet<StripeCheckout> StripeCheckouts { get; set; }

    public  DbSet<License> Licenses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Company>().HasKey(c => c.Id);
        modelBuilder.Entity<Company>().Property(c => c.StripeCustomerId).HasMaxLength(450).IsRequired(false);
        modelBuilder.Entity<Company>().Property(c => c.CompanyName).HasMaxLength(200).IsRequired();
        modelBuilder.Entity<Company>()
            .HasOne(c => c.License)
            .WithOne(ps => ps.Company)
            .HasForeignKey<License>(ps => ps.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Company>().HasData(new Company
        {
            Id = new Guid("C4D8BA8D-ACE3-442E-A84D-D72D3F4D6F29"),
            CompanyName = "Test Company",
            StripeCustomerId = null,
        });


        modelBuilder.Entity<LicenseType>().HasKey(s => s.Id);
        modelBuilder.Entity<LicenseType>().Property(s => s.StripePriceId).HasMaxLength(450).IsRequired();
        modelBuilder.Entity<LicenseType>().Property(s => s.Price).IsRequired().HasPrecision(14, 2);
        modelBuilder.Entity<LicenseType>().Property(s => s.Description).IsRequired().HasMaxLength(200);
        modelBuilder.Entity<LicenseType>().Property(s => s.Name).IsRequired().HasMaxLength(200);
        modelBuilder.Entity<LicenseType>().HasData(new List<LicenseType>
        {
            new()
            {
                Id = new Guid("CDB8E8AC-27E8-430A-B67D-03C2F3E85273"),
                StripePriceId = "YOUR_PRICE_ID",
                Description = "This is a standard test subscription",
                Name = "Standard Subscription",
                Price = 20.00m
            },
            new()
            {
                Id = new Guid("846C0B14-BA0C-4757-8D7F-40D895A7EBC2"),
                StripePriceId = "YOUR_PRICE_ID",
                Description = "This is a premium subscription",
                Name = "Premium Subscription",
                Price = 50.00m
            }
        });

        modelBuilder.Entity<StripeCheckout>().HasKey(sc => sc.Id);
        modelBuilder.Entity<StripeCheckout>().Property(sc => sc.CompanyId).IsRequired().HasMaxLength(450);
        modelBuilder.Entity<StripeCheckout>().Property(sc => sc.StripeSessionId).IsRequired().HasMaxLength(450);
        modelBuilder.Entity<StripeCheckout>().Property(sc => sc.LicenceTypeId).IsRequired().HasMaxLength(450);
        modelBuilder.Entity<StripeCheckout>().Property(sc => sc.CreatedAt).IsRequired();
        modelBuilder.Entity<StripeCheckout>().Property(sc => sc.Status).IsRequired().HasMaxLength(50);

        modelBuilder.Entity<License>().HasKey(ps => ps.Id);
        modelBuilder.Entity<License>().Property(ps => ps.CompanyId).IsRequired();
        modelBuilder.Entity<License>().Property(ps => ps.LicenceTypeId).IsRequired();
        modelBuilder.Entity<License>().Property(ps => ps.ExpiredAt).IsRequired();
        modelBuilder.Entity<License>()
            .HasOne(ps => ps.LicenseType)
            .WithMany(s => s.Licenses)
            .HasForeignKey(ps => ps.LicenceTypeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}