namespace Brokers.RabbitMq;

public class RabbitMqOptions
{
    public required RabbitMqSettings Settings { get; set; }
    public required string? SelectedQueue { get; set; }
}
