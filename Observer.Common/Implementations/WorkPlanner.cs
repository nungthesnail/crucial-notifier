using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Observer.Common.Interfaces.ObservationDispatcher;

namespace Observer.Common.Implementations;

public class WorkPlanner : BackgroundService
{
    private static int _iterationId;
    private readonly ILogger<WorkPlanner> _logger;
    private readonly IObservationDispatcherFactory _dispatcherFactory;
    private readonly double _workIntervalMinutes;
    
    public WorkPlanner(ILogger<WorkPlanner> logger, IObservationDispatcherFactory dispatcherFactory,
        IConfiguration config)
    {
        const string workIntervalKey = "WorkIntervalMinutes";
        
        _logger = logger;
        _dispatcherFactory = dispatcherFactory;
        _workIntervalMinutes = config.GetValue<double>(workIntervalKey);
        
        _logger.LogInformation("Work planner created. Work interval: {interval} minutes", _workIntervalMinutes);
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var interval = TimeSpan.FromMinutes(_workIntervalMinutes);
        var timer = new PeriodicTimer(interval);

        do
        {
            await PerformIterationAsync(stoppingToken);
        } while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken));
    }

    private async Task PerformIterationAsync(CancellationToken stoppingToken)
    {
        var iterationId = Interlocked.Increment(ref _iterationId);
        try
        {
            _logger.LogInformation("Starting iteration {iterationId}", iterationId);

            var dispatcher = _dispatcherFactory.CreateObservationDispatcher();
            await dispatcher.ObserveAsync(stoppingToken);
            
            _logger.LogInformation("Iteration {iterationId} completed", iterationId);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Iteration {iterationId} is cancelled", iterationId);
        }
        catch (Exception exc)
        {
            _logger.LogError(exc, "Something failed during iteration {iterationId}", iterationId);
        }
    }
}
