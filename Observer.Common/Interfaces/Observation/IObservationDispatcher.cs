namespace Observer.Common.Interfaces.Observation;

public interface IObservationDispatcher
{
    Task ObserveAsync(CancellationToken stoppingToken);
}