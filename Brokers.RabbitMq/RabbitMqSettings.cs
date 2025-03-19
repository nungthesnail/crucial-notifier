using Brokers.Common;

namespace Brokers.RabbitMq;

public class RabbitMqSettings
{
    private const string DefaultQueueNameValue = "schedule_observation";
    
    public required RabbitMqConnectionSettings ConnectionSettings { get; set; }
    public RabbitMqQueueSettings QueueSettings { get; set; } = new();
    public Dictionary<string, string> QueueNames { get; set; } = [];

    public string DefaultQueueName
    {
        get => QueueNames.GetValueOrDefault(TopicNamesKeys.Default, DefaultQueueNameValue);
        set => QueueNames[TopicNamesKeys.Default] = value;
    }
}
