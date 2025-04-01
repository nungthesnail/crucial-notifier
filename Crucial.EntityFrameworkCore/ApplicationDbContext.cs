using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crucial.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public DbSet<CourseModel> Courses { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureCourses(modelBuilder.Entity<CourseModel>());
    }

    private static void ConfigureCourses(EntityTypeBuilder<CourseModel> builder)
    {
        builder.ToTable("course");
        builder.HasKey(static x => x.Id);
        
        builder
            .HasIndex(static x => x.Name)
            .IsUnique();
        
        builder
            .Property(static x => x.Id)
            .HasField("id")
            .IsRequired();
        builder
            .Property(static x => x.Name)
            .HasField("name")
            .HasMaxLength(128)
            .IsRequired();
        builder
            .Property(static x => x.Description)
            .HasField("description")
            .HasMaxLength(512);
        builder
            .Property(static x => x.TotalLessonsCount)
            .HasField("total_lessons_count")
            .IsRequired();
        builder
            .Property(static x => x.LessonsPassedCount)
            .HasField("lessons_passed_count")
            .HasDefaultValue(0)
            .IsRequired();
    }
}
