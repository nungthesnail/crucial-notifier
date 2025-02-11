namespace Observer.Common.Interfaces.Observation;

public interface IObservationDispatcherFactory
{
    IObservationDispatcher CreateObservationDispatcher();
}