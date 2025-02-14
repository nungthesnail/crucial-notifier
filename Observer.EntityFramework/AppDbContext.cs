using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Observer.EntityFramework.Converters;
using Observer.EntityFramework.Models;

namespace Observer.EntityFramework;

public class AppDbContext : DbContext
{
    public DbSet<HistoryStamp> HistoryStamps { get; set; }
    public DbSet<Recipient> Recipients { get; set; }
    public DbSet<NotificationSendingEvent> NotificationSendingEvents { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureHistoryStamp(modelBuilder.Entity<HistoryStamp>());
        ConfigureRecipient(modelBuilder.Entity<Recipient>());
        ConfigureNotificationSendingEvent(modelBuilder.Entity<NotificationSendingEvent>());
        return;
        
        static void ConfigureHistoryStamp(EntityTypeBuilder<HistoryStamp> entityBuilder)
        {
            entityBuilder
                .ToTable("history_stamp")
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

        static void ConfigureRecipient(EntityTypeBuilder<Recipient> entityBuilder)
        {
            entityBuilder
                .ToTable("recipient")
                .HasKey(static x => x.Id);
            entityBuilder
                .Property(static x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd()
                .IsRequired();
            entityBuilder
                .Property(static x => x.Identifier)
                .HasColumnName("identifier")
                .HasMaxLength(256);
            entityBuilder
                .Property(static x => x.Type)
                .HasColumnName("type")
                .IsRequired();
            entityBuilder
                .Property(static x => x.Active)
                .HasColumnName("active")
                .HasDefaultValueSql("true")
                .IsRequired();
        }

        static void ConfigureNotificationSendingEvent(EntityTypeBuilder<NotificationSendingEvent> entityBuilder)
        {
            entityBuilder
                .ToTable("notification_sending_event")
                .HasKey(static x => x.Id);
            entityBuilder
                .Property(static x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd()
                .IsRequired();
            entityBuilder
                .Property(static x => x.Sent)
                .HasColumnName("sent")
                .HasDefaultValueSql("false")
                .IsRequired();
            entityBuilder
                .Property(static x => x.Timestamp)
                .HasColumnName("ts")
                .HasDefaultValueSql("now()")
                .IsRequired();
        }
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<DateTimeOffset>()
            .HaveConversion<DateTimeOffsetConverter>();
    }
}
