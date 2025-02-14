using Microsoft.Extensions.Logging;
using Observer.Common.Interfaces.Observation;
using Observer.Common.Interfaces.Services;

namespace Observer.Common.Implementations.Observation;

public sealed class ObservationDispatcher : IObservationDispatcher
{
    private readonly ILogger<ObservationDispatcher> _logger;
    private readonly IContentProvider _contentProvider;
    private readonly IHistoryRepository _historyRepository;
    private readonly IDataComparer _dataComparer;
    private readonly IResultHandler _resultHandler;

    public ObservationDispatcher(ILogger<ObservationDispatcher> logger, IContentProvider contentProvider,
        IHistoryRepository historyRepository, IDataComparer dataComparer, IResultHandler resultHandler)
    {
        _logger = logger;
        _contentProvider = contentProvider;
        _historyRepository = historyRepository;
        _dataComparer = dataComparer;
        _resultHandler = resultHandler;
    }

    public IDisposable? DisposableResources { get; set; }

    public async Task ObserveAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Dispatching observation...");
        var currentData = await _contentProvider.GetContentAsync(stoppingToken);
        var previousData = _historyRepository.GetLastContent(stoppingToken);
        await _historyRepository.AddHistoryStampAsync(currentData, stoppingToken);
        var comparisonResult = _dataComparer.Compare(currentData, previousData);
        await _resultHandler.HandleResultAsync(comparisonResult, stoppingToken);
        
        _logger.LogInformation("Disposing dispatcher resources...");
        Dispose();
    }

    public void Dispose()
    {
        DisposableResources?.Dispose();
    }
}
