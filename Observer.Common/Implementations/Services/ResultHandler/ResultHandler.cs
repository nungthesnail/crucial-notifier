using Microsoft.Extensions.Logging;
using Observer.Common.Interfaces.Services;
using Observer.Common.Models;

namespace Observer.Common.Implementations.Services.ResultHandler;

public class ResultHandler : IResultHandler
{
    private readonly ILogger<ResultHandler> _logger;
    private readonly INotifier _notifier;
    private readonly INotifierTaskBuilder _taskBuilder;

    public ResultHandler(ILogger<ResultHandler> logger, INotifier notifier, INotifierTaskBuilder taskBuilder)
    {
        _logger = logger;
        _notifier = notifier;
        _taskBuilder = taskBuilder;
    }
    
    public async Task HandleResultAsync(ComparisonResult result, CancellationToken stoppingToken)
    {
        _logger.LogInformation("Handling result of comparison...");
        _logger.LogInformation("Making a decision to send notifications.");
        if (!DecideToSend(result))
        {
            _logger.LogInformation("Negative decision was made. Aborting result handling.");
            return;
        }
        _logger.LogInformation("Positive decision was made. Sending notifications.");
        
        var notifierTask = await _taskBuilder.BuildAsync(result, stoppingToken);
        await _notifier.SendAsync(notifierTask, stoppingToken);
        
        _logger.LogInformation("Handling result completed.");
    }

    private static bool DecideToSend(ComparisonResult result)
        => !result.HashesIdentical || !result.ModifiedTimestampsIdentical;
}