using Microsoft.EntityFrameworkCore;

namespace Crucial.EntityFrameworkCore.Tests;

[TestFixture]
public class ApplicationDbContextTests
{
    private const string ConnectionString = "Data Source=appdata.db";
    
    [Test]
    public async Task TestInitializeDbContext_CoursesNotNull_Async()
    {
        // Arrange
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlite(ConnectionString);

        // Act
        await using var dbContext = new ApplicationDbContext(optionsBuilder.Options);
        var allCourses = await dbContext.Courses.ToListAsync();

        // Assert
        Assert.That(allCourses, Is.Not.Null);
    }
}
