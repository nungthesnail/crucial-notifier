using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Site.Data.Models;

namespace Web.Site.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<Subscriptition> Subscriptitions { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        ConfigureSubscriptitions(builder.Entity<Subscriptition>());
        base.OnModelCreating(builder);
    }

    private static void ConfigureSubscriptitions(EntityTypeBuilder<Subscriptition> entity)
    {
        entity.ToTable("subscriptitions");
        entity.HasKey(static x => x.Id);
        entity
            .HasOne(static x => x.User)
            .WithMany();
        
        entity.Property(static x => x.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();
        entity.Property(static x => x.UserId)
            .HasMaxLength(64)
            .IsRequired();
        entity.Property(static x => x.Active)
            .HasDefaultValueSql("false")
            .IsRequired();
    }
}
