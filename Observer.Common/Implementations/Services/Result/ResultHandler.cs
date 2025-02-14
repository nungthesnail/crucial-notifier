using Microsoft.Extensions.Logging;
using Observer.Common.Interfaces.Services;
using Observer.Common.Models;

namespace Observer.Common.Implementations.Services.Result;

public sealed class ResultHandler : IResultHandler
{
    private readonly ILogger<ResultHandler> _logger;
    private readonly INotifier _notifier;
    private readonly INotifierTaskBuilder _taskBuilder;
    private readonly IHistoryRepository _historyRepository;

    public ResultHandler(ILogger<ResultHandler> logger, INotifier notifier, INotifierTaskBuilder taskBuilder,
        IHistoryRepository historyRepository)
    {
        _logger = logger;
        _notifier = notifier;
        _taskBuilder = taskBuilder;
        _historyRepository = historyRepository;
    }
    
    public async Task HandleResultAsync(ComparisonResult result, CancellationToken stoppingToken)
    {
        _logger.LogInformation("Handling result of comparison...");
        _logger.LogInformation("Making a decision to send notifications.");
        if (!DecideToSend(result))
        {
            await _historyRepository.AddSendingEventAsync(false, stoppingToken);
            _logger.LogInformation("Negative decision was made and record about decision was added to db. " +
                                   "Aborting result handling.");
            return;
        }
        _logger.LogInformation("Positive decision was made. Sending notifications and adding record about sending into db");
        
        var notifierTask = await _taskBuilder.BuildAsync(result, stoppingToken);
        var taskNotifier = _notifier.SendAsync(notifierTask, stoppingToken);
        var taskHistory = _historyRepository.AddSendingEventAsync(true, stoppingToken);
        
        Task.WaitAll([taskNotifier, taskHistory], stoppingToken);
        
        _logger.LogInformation("Handling result completed.");
    }

    private static bool DecideToSend(ComparisonResult result)
        => !result.HashesIdentical || !result.ModifiedTimestampsIdentical;
}
