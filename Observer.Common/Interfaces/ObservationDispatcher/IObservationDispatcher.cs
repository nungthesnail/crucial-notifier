namespace Observer.Common.Interfaces.ObservationDispatcher;

public interface IObservationDispatcher
{
    Task ObserveAsync(CancellationToken stoppingToken);
}