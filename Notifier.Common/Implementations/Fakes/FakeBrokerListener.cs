using Microsoft.Extensions.Hosting;
using Notifier.Common.Interfaces.Message;

namespace Notifier.Common.Implementations.Fakes;

public class FakeBrokerListener : BackgroundService
{
    private event MessageHandler OnMessageReceived;
    private readonly int _iterationLimit;
    
    public FakeBrokerListener(IMessageHandler messageHandler, int iterationLimit = 1)
    {
        OnMessageReceived += messageHandler.Handle;
        _iterationLimit = iterationLimit;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        const int timerIntervalSeconds = 5;
        var iteration = 0;
        var timer = new PeriodicTimer(TimeSpan.FromSeconds(timerIntervalSeconds));

        do
        {
            await OnMessageReceived.Invoke(GetMessage(iteration));
            ++iteration;
        } while (
            !stoppingToken.IsCancellationRequested
            && await timer.WaitForNextTickAsync(stoppingToken)
            && iteration < _iterationLimit);
    }

    private static string GetMessage(int number) => $"Message number {number}";
    
    private delegate Task MessageHandler(string message);
}
