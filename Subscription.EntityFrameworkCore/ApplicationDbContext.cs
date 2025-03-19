using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Subscription.Model;

namespace Subscription.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public DbSet<SubscriptionModel> Subscriptions { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureSubscriptitions(modelBuilder.Entity<SubscriptionModel>());
    }

    private static void ConfigureSubscriptitions(EntityTypeBuilder<SubscriptionModel> entity)
    {
        entity.ToTable("subscriptitions");
        entity.HasKey(static x => x.Id);
        
        entity
            .Property(static x => x.Id)
            .IsRequired();
        
        entity
            .Property(static x => x.UserId)
            .HasMaxLength(36)
            .IsRequired();
        
        entity
            .Property(static x => x.Active)
            .IsRequired();
        
        entity
            .Property(static x => x.Email)
            .HasMaxLength(100)
            .IsRequired();
    }
}
