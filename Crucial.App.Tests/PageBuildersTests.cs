using Crucial.App.Interfaces;
using Crucial.App.Models;
using Crucial.App.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Crucial.App.Tests;

[TestFixture]
public class PageBuildersTests : TestsWithConcreteServicesBase
{
    [TestCase(PageName.Main, typeof(MainView))]
    public void TestCreateForPagesWithNoParameters_InputIsEmptyArguments_ReturnsCorrespondingView(
        PageName pageName, Type expectedViewType)
    {
        IServiceScope? scope = null;
        try
        {
            // Arrange
            var services = PrepareServices();
            scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var pageBuilder = scope.ServiceProvider.GetRequiredKeyedService<IPageBuilder>(pageName);
            pageBuilder.SetArguments([]);
            
            // Act
            var page = pageBuilder.Build();

            // Assert
            Assert.That(page, Is.AssignableTo(expectedViewType));
        }
        finally
        {
            scope?.Dispose();
        }
    }
}
