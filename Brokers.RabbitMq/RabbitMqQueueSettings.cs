namespace Brokers.RabbitMq;

public class RabbitMqQueueSettings
{
    public bool Durable { get; set; }
    public bool Exclusive { get; set; }
    public bool AutoDelete { get; set; }
    public Dictionary<string, object?>? Arguments { get; set; }
}
