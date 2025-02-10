using Observer.Common.Interfaces.ObservationDispatcher;
using Observer.Common.Interfaces.Services;

namespace Observer.Common.Implementations.ObservationDispatcher;

public class ObservationDispatcher : IObservationDispatcher
{
    private readonly IContentProvider _contentProvider;
    private readonly IHistoryProvider _historyProvider;
    private readonly IDataComparer _dataComparer;
    private readonly IResultHandler _resultHandler;

    public ObservationDispatcher(IContentProvider contentProvider, IHistoryProvider historyProvider,
        IDataComparer dataComparer, IResultHandler resultHandler)
    {
        _contentProvider = contentProvider;
        _historyProvider = historyProvider;
        _dataComparer = dataComparer;
        _resultHandler = resultHandler;
    }
    
    public async Task ObserveAsync(CancellationToken stoppingToken)
    {
        var currentData = await _contentProvider.GetContentAsync(stoppingToken);
        var previousData = _historyProvider.GetLastContent(stoppingToken);
        var comparisonResult = _dataComparer.Compare(currentData, previousData);
        await _resultHandler.HandleResultAsync(comparisonResult);
    }
}
