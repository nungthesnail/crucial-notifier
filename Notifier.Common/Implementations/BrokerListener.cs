using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Notifier.Common.Interfaces.Message;
using Notifier.Common.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Notifier.Common.Implementations;

public class BrokerListener : BackgroundService
{
    private event MessageHandler OnMessageReceived;
    
    private readonly ILogger<BrokerListener> _logger;
    
    private readonly BrokerListenerSettings _settings = new();
    private IConnection _connection = null!;
    private IChannel _channel = null!;

    private delegate Task MessageHandler(string message);
    
    public BrokerListener(ILogger<BrokerListener> logger, IMessageHandler messageHandler, IConfiguration config)
    {
        _logger = logger;
        OnMessageReceived += messageHandler.Handle;
        config.GetSection("RabbitMQ").Bind(_settings);
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await InitializeRabbitMq();
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (channel, message) =>
        {
            var content = Encoding.UTF8.GetString(message.Body.ToArray());
            _logger.LogTrace("Received a message from RabbitMQ: {content}", content);
            _ = OnMessageReceived.Invoke(content);
            await _channel.BasicAckAsync(message.DeliveryTag, false, stoppingToken);
        };

        await _channel.BasicConsumeAsync(_settings.QueueName, false, consumer, stoppingToken);
    }

    private async Task InitializeRabbitMq()
    {
        var factory = new ConnectionFactory { HostName = _settings.HostName };
        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
        await _channel.QueueDeclareAsync(
            queue: _settings.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false);
    }

    public override void Dispose()
    {
        Task.WaitAll(
            _channel.CloseAsync(),
            _connection.CloseAsync());
        base.Dispose();
    }
}