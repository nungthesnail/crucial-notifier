using Avalonia.Controls;
using Crucial.App.Implementations;
using Crucial.App.Interfaces;
using Crucial.App.Models;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Crucial.App.Tests;

[TestFixture]
public class PageFactoryTests
{
    [Test]
    public void TestCreate_InputIsMainPageAndWellRegisteredServicesAndValidArgs_ReturnsFakeMainPage()
    {
        IServiceScope? scope = null;

        try
        {
            // Arrange
            const PageName pageName = PageName.Main;

            var fakeMainPage = new UserControl();
            var pageBuilderStub = new Mock<IPageBuilder>();
            pageBuilderStub.Setup(static x => x.Build())
                .Returns(fakeMainPage);

            var serviceProvider = new ServiceCollection()
                .AddKeyedTransient<UserControl>(pageName, (_, _) => fakeMainPage)
                .AddKeyedTransient<IPageBuilder>(pageName, (_, _) => pageBuilderStub.Object)
                .BuildServiceProvider();

            // Act
            (var mainPage, scope) = PageFactory.Create(pageName, serviceProvider);

            // Assert
            Assert.That(mainPage, Is.SameAs(fakeMainPage));
        }
        finally
        {
            scope?.Dispose();
        }
    }
}
