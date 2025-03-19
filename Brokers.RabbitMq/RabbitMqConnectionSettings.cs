namespace Brokers.RabbitMq;

public class RabbitMqConnectionSettings
{
    public required string Host { get; set; }
    public int Port { get; set; }
    public required string Username { get; set; }
    public required string Password { internal get; set; }
}