namespace Observer.Common.Implementations.Fakes.Configuration;

public static class FakeConfigurationProvider
{
    public static IEnumerable<KeyValuePair<string, string?>> GetFakeConfiguration()
    {
        return new List<KeyValuePair<string, string?>>
        {
            new("ConnectionStrings:Postgres",
                "Server=localhost;Port=5432;Database=crucial_notifier;Timeout=1000;CommandTimeout=1000;User Id=postgres;Password=123;ApplicationName=Observer;Pooling=true;MinPoolSize=1;MaxPoolSize=100;"),
            new("WorkInterval", "10"),
            new("ObservingUrl", "https://scheduleavto.github.io/hg.htm"),
            new("HtmlParsing:InterestingAttribute", "class"),
            new("HtmlParsing:InterestingAttributeValue", "ref"),
            new("RabbitMq:HostName", "localhost"),
            new("RabbitMq:Exchange", ""),
            new("RabbitMq:QueueName", "schedule_observation"),
            new("RabbitMq:RoutingKey", "schedule_observation")
        };
    }
}