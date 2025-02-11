using System.Text;
using Microsoft.Extensions.Configuration;
using Observer.Common.Exceptions;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Observer.Common.Implementations.Services.Notifier;

public sealed class RabbitMqSender
{
    private readonly RabbitMqSenderSettings _settings = new();

    public RabbitMqSender(IConfiguration config)
    {
        config.GetSection("RabbitMq").Bind(_settings);
    }
    
    public async Task SendMessageAsync(string message, CancellationToken stoppingToken)
    {
        try
        {
            stoppingToken.ThrowIfCancellationRequested();
            
            var factory = new ConnectionFactory
            {
                HostName = _settings.HostName
            };

            await using var connection = await factory.CreateConnectionAsync(cancellationToken: stoppingToken);
            await using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

            await channel.QueueDeclareAsync(
                queue: _settings.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: stoppingToken);
            
            var messageBytes = Encoding.UTF8.GetBytes(message);
            
            await channel.BasicPublishAsync(
                exchange: _settings.Exchange,
                routingKey: _settings.RoutingKey,
                body: messageBytes,
                cancellationToken: stoppingToken);
        }
        catch (BrokerUnreachableException)
        {
            throw new RabbitMqException("Broker is currently unreachable");
        }
    }
}