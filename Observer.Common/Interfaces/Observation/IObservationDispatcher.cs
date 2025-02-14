namespace Observer.Common.Interfaces.Observation;

public interface IObservationDispatcher : IDisposable
{
    Task ObserveAsync(CancellationToken stoppingToken);
    internal IDisposable? DisposableResources { set; }
}
