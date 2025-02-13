namespace Observer.Common.Implementations.Services.Notifier;

public class RabbitMqSenderSettings
{
    public string Host { get; set; } = string.Empty;
    public string Queue { get; set; } = string.Empty;
    public string Exchange { get; set; } = string.Empty;
    public string RoutingKey { get; set; } = string.Empty;
    
    public string User { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}