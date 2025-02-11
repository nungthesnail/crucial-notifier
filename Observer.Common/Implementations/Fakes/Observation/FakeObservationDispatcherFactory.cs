using Microsoft.Extensions.DependencyInjection;
using Observer.Common.Interfaces.Observation;

namespace Observer.Common.Implementations.Fakes.Observation;

public class FakeObservationDispatcherFactory : IObservationDispatcherFactory
{
    private readonly IServiceProvider _serviceProvider;

    public FakeObservationDispatcherFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public IObservationDispatcher CreateObservationDispatcher()
    {
        return _serviceProvider.GetRequiredService<IObservationDispatcher>();
    }
}