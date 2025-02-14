using Microsoft.Extensions.DependencyInjection;
using Observer.Common.Interfaces.Observation;

namespace Observer.Common.Implementations.Observation;

public class ObservationDispatcherFactory : IObservationDispatcherFactory
{
    private readonly IServiceScopeFactory _scopeFactory;
    
    public ObservationDispatcherFactory(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public IObservationDispatcher CreateObservationDispatcher()
    {
        var scope = _scopeFactory.CreateScope();
        var dispatcher = scope.ServiceProvider.GetRequiredService<IObservationDispatcher>();
        dispatcher.DisposableResources = scope;
        return dispatcher;
    }
}
