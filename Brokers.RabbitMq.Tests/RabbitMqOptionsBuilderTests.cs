using NUnit.Framework;

namespace Brokers.RabbitMq.Tests;

public class RabbitMqOptionsBuilderTests
{
    private readonly RabbitMqSettings _defaultSettings = new()
    {
        ConnectionSettings = new RabbitMqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            Username = "guest",
            Password = "password"
        }
    };
    [Test]
    public void TestBuild_AllDependenciesSpecified_ResultIsCorrectRabbitMqOptions()
    {
        // Arrange
        const string queueName = "used-queue";
        var builder = new RabbitMqOptionsBuilder()
            .UseSettings(_defaultSettings)
            .UseQueue(queueName);

        // Act
        var options = builder.Build();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(options.Settings, Is.EqualTo(_defaultSettings));
            Assert.That(options.SelectedQueue, Is.EqualTo(queueName));
        });
    }

    [Test]
    public void TestUseDefaultQueue_ResultIsOptionsWithNullSelectedQueue()
    {
        // Arrange
        var builder = new RabbitMqOptionsBuilder()
            .UseSettings(_defaultSettings)
            .UseQueue("some-queue");
        
        // Act
        var options = builder
            .UseDefaultQueue()
            .Build();
        
        // Assert
        Assert.That(options.SelectedQueue, Is.Null);
    }

    [Test]
    public void TestBuild_SettingsArentSpecified_ThrowsInvalidOperationException()
    {
        // Arrange
        var builder = new RabbitMqOptionsBuilder();

        // Act
        var throwingFunc = () => builder.Build();

        // Assert
        Assert.That(throwingFunc, Throws.InvalidOperationException);
    }
}
