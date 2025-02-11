using Observer.Common.Implementations.Fakes.Observation;

namespace Observer.Common.Tests.General;

public sealed class GeneralWorkTests
{
    [Test]
    public Task GeneralWorkTest_NoIntegrations_VolatileContentProvider_OneIteration_Async()
    {
        var factoryBuilder = new FakeObservationDispatcherFactoryBuilder();
        factoryBuilder.NotIntegrateDatabase();
        factoryBuilder.NotIntegrateObservationSubject();
        factoryBuilder.NotIntegrateNotifier();
        factoryBuilder.SetObservationSubjectStubType(
            FakeObservationDispatcherFactoryBuilder.ObservationSubjectStubType.Volatile);

        var dispatcherFactory = factoryBuilder.Build();
        var dispatcher = dispatcherFactory.CreateObservationDispatcher();
        var cts = new CancellationTokenSource();
        return dispatcher.ObserveAsync(cts.Token);
    }

    [Test]
    public async Task GeneralWorkTest_NoIntegrations_VolatileContentProvider_TwoIterations_Async()
    {
        var factoryBuilder = new FakeObservationDispatcherFactoryBuilder();
        factoryBuilder.NotIntegrateDatabase();
        factoryBuilder.NotIntegrateObservationSubject();
        factoryBuilder.NotIntegrateNotifier();
        factoryBuilder.SetObservationSubjectStubType(
            FakeObservationDispatcherFactoryBuilder.ObservationSubjectStubType.Volatile);

        var dispatcherFactory = factoryBuilder.Build();
        var dispatcher = dispatcherFactory.CreateObservationDispatcher();
        var cts = new CancellationTokenSource();
        await dispatcher.ObserveAsync(cts.Token);
        await dispatcher.ObserveAsync(cts.Token);
    }
}