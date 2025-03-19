using System.Text;
using Brokers.Common;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Brokers.RabbitMq;

public class RabbitMqMessageConsumer : IConsumer, IDisposable
{
    private readonly RabbitMqSettings _settings;
    private readonly string _queueName;

    private IConnection _connection = null!;
    private IChannel _channel = null!;
    private bool _initialized;
    
    public event MessageReceivedAsyncEventHandler? OnMessageReceived;

    public RabbitMqMessageConsumer(RabbitMqOptions options)
    {
        _settings = options.Settings;
        _queueName = options.SelectedQueue ?? _settings.DefaultQueueName;
    }

    public RabbitMqMessageConsumer(RabbitMqSettings settings)
    {
        _settings = settings;
        _queueName = settings.DefaultQueueName;
    }
    
    public async Task StartConsumingAsync(CancellationToken stoppingToken)
    {
        if (!_initialized)
            await InitializeAsync(stoppingToken);
        
        stoppingToken.ThrowIfCancellationRequested();
        
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, message) =>
        {
            var content = Encoding.UTF8.GetString(message.Body.ToArray());
            OnMessageReceived?.Invoke(new MessageReceivedEventArgs
            {
                Message = content
            });
            await _channel.BasicAckAsync(message.DeliveryTag, false, stoppingToken);
        };
        
        await _channel.BasicConsumeAsync(_queueName, false, consumer, stoppingToken);
    }

    public async Task InitializeAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        
        var factory = new ConnectionFactory
        {
            HostName = _settings.ConnectionSettings.Host,
            Port = _settings.ConnectionSettings.Port,
            UserName = _settings.ConnectionSettings.Username,
            Password = _settings.ConnectionSettings.Password
        };
        
        _connection = await factory.CreateConnectionAsync(stoppingToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);
        await _channel.QueueDeclareAsync(
            queue: _queueName,
            durable: _settings.QueueSettings.Durable,
            exclusive: _settings.QueueSettings.Exclusive,
            autoDelete: _settings.QueueSettings.AutoDelete,
            cancellationToken: stoppingToken);
        
        _initialized = true;
    }

    public void Dispose()
    {
        Task.WaitAll(
            _channel.CloseAsync(),
            _connection.CloseAsync());
        GC.SuppressFinalize(this);
    }
}
