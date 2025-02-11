using Microsoft.Extensions.DependencyInjection;
using Observer.Common.Interfaces.ObservationDispatcher;

namespace Observer.Common.Implementations.ObservationDispatcher;

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