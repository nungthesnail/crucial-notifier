using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crucial.EntityFrameworkCore;

public sealed class ApplicationDbContext : DbContext
{
    public DbSet<CourseModel> Courses { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        #if IOS
        SQLitePCL.Batteries_V2.Init();
        #endif
        
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureCourses(modelBuilder.Entity<CourseModel>());
    }

    private static void ConfigureCourses(EntityTypeBuilder<CourseModel> builder)
    {
        builder
            .HasIndex(static x => x.Name)
            .IsUnique();
    }
}
