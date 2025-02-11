using Microsoft.Extensions.DependencyInjection;
using Observer.Common.Interfaces.Observation;

namespace Observer.Common.Implementations.Observation;

public class ObservationDispatcherFactory : IObservationDispatcherFactory
{
    private readonly IServiceProvider _serviceProvider;
    
    public ObservationDispatcherFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IObservationDispatcher CreateObservationDispatcher()
        => _serviceProvider.GetRequiredService<IObservationDispatcher>();
}