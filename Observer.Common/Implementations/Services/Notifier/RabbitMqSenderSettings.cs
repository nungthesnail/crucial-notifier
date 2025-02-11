namespace Observer.Common.Implementations.Services.Notifier;

public class RabbitMqSenderSettings
{
    public string HostName { get; set; } = string.Empty;
    public string QueueName { get; set; } = string.Empty;
    public string Exchange { get; set; } = string.Empty;
    public string RoutingKey { get; set; } = string.Empty;
}