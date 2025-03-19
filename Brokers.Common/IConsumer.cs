namespace Brokers.Common;

public interface IConsumer
{
    event MessageReceivedAsyncEventHandler? OnMessageReceived;
    Task StartConsumingAsync(CancellationToken stoppingToken);
}
