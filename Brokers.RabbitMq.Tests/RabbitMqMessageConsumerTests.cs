using NUnit.Framework;

namespace Brokers.RabbitMq.Tests;

public class RabbitMqMessageConsumerTests
{
    private readonly RabbitMqSettings _settings = new()
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
    public void TestConstructor_InputIsRabbitMqSettings_ThrowsNothing()
    {
        // Arrange
        
        // Act
        var throwsNothing = () => new RabbitMqMessageConsumer(_settings);
        
        // Assert
        Assert.That(throwsNothing, Throws.Nothing);
    }

    [Test]
    public void TestConstructor_InputIsRabbitMqOptions_ThrowsNothing()
    {
        // Arrange
        var options = new RabbitMqOptionsBuilder()
            .UseSettings(_settings)
            .Build();
        
        // Act
        var throwsNothing = () => new RabbitMqMessageConsumer(options);
        
        // Assert
        Assert.That(throwsNothing, Throws.Nothing);
    }
}
