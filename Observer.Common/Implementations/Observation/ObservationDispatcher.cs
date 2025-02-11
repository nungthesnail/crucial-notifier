using Observer.Common.Interfaces.Observation;
using Observer.Common.Interfaces.Services;

namespace Observer.Common.Implementations.Observation;

public class ObservationDispatcher : IObservationDispatcher
{
    private readonly IContentProvider _contentProvider;
    private readonly IHistoryRepository _historyRepository;
    private readonly IDataComparer _dataComparer;
    private readonly IResultHandler _resultHandler;

    public ObservationDispatcher(IContentProvider contentProvider, IHistoryRepository historyRepository,
        IDataComparer dataComparer, IResultHandler resultHandler)
    {
        _contentProvider = contentProvider;
        _historyRepository = historyRepository;
        _dataComparer = dataComparer;
        _resultHandler = resultHandler;
    }
    
    public async Task ObserveAsync(CancellationToken stoppingToken)
    {
        var currentData = await _contentProvider.GetContentAsync(stoppingToken);
        var previousData = _historyRepository.GetLastContent(stoppingToken);
        var addStampTask = _historyRepository.AddHistoryStampAsync(currentData, stoppingToken);
        var comparisonResult = _dataComparer.Compare(currentData, previousData);
        var handlingResultTask = _resultHandler.HandleResultAsync(comparisonResult, stoppingToken);
        await Task.WhenAll(addStampTask, handlingResultTask);
    }
}
