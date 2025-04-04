using Crucial.Core.Interfaces.Dal;
using Crucial.Core.Interfaces.Managers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crucial.App.Tests;

[TestFixture]
public class StartupTests
{
    private readonly IConfiguration _config
        = new ConfigurationBuilder()
        .AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["ConnectionStrings:Default"] = "Data Source=unused.db"
        })
        .Build();
    
    [Test]
    public void TestAddCommonServices_AllServicesRegisteredAndAbleToCreate()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddCommonServices(_config);
        var provider = services.BuildServiceProvider();
        
        var courseManager = provider.GetRequiredService<ICourseManager>();
        var lessonManager = provider.GetRequiredService<ILessonManager>();
        var unitOfWork = provider.GetRequiredService<IUnitOfWork>();
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(courseManager, Is.Not.Null);
            Assert.That(lessonManager, Is.Not.Null);
            Assert.That(unitOfWork, Is.Not.Null);
        });
    }
}
