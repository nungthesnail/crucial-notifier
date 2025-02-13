using Microsoft.Extensions.Configuration;
using Observer.Common.Implementations.Services.Notifier;

namespace Observer.Common.Tests.Integration;

public class RabbitMqSimpleIntegrationTest
{
    [Test]
    public Task TestRabbitMqIntegration()
    {
        var testMessage =
            $$"""{"Schema": "", "Message": "Hello, RabbitMQ!", "SendingTime": "{{DateTimeOffset.Now}}"}""";
        
        var config = CreateConfiguration();
        var sender = new RabbitMqSender(config);
        var cts = new CancellationTokenSource();
        return sender.SendMessageAsync(testMessage, cts.Token);
    }

    private static IConfiguration CreateConfiguration()
    {
        var builder = new ConfigurationBuilder();

        var settings = new List<KeyValuePair<string, string?>>
        {
            new("RabbitMq:Host", "localhost"),
            new("RabbitMq:Exchange", ""),
            new("RabbitMq:Queue", "schedule_observation"),
            new("RabbitMq:RoutingKey", "schedule_observation"),
            new("RabbitMq:User", "mquser"),
            new("RabbitMq:Password", "123456"),
        };
        
        builder.AddInMemoryCollection(settings);
        return builder.Build();
    }
}
