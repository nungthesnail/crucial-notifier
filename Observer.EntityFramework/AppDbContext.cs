using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using Observer.EntityFramework.Exceptions;
using Observer.EntityFramework.Models;

namespace Observer.EntityFramework;

public class AppDbContext : DbContext
{
    private readonly string _connectionString;
    public DbSet<HistoryStamp> HistoryStamps { get; set; }

    public AppDbContext(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("Postgres")
                            ?? throw new BadConfigurationException("Postgres connection string isn't specified");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureHistoryStamp(modelBuilder.Entity<HistoryStamp>());
        return;
        
        static void ConfigureHistoryStamp(EntityTypeBuilder<HistoryStamp> entityBuilder)
        {
            entityBuilder
                .HasKey(static x => x.Id);
            entityBuilder
                .Property(static x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd()
                .IsRequired();
            entityBuilder
                .Property(static x => x.Hash)
                .HasColumnName("hash")
                .HasMaxLength(64)
                .IsRequired();
            entityBuilder
                .Property(static x => x.LastModified)
                .HasColumnName("last_modified_ts");
            entityBuilder
                .Property(static x => x.InsertedAt)
                .HasColumnName("inserted_at_ts")
                .HasDefaultValueSql("now()");
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
    }
}