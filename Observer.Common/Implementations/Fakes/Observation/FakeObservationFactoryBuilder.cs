using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Observer.Common.Implementations.Fakes.Configuration;
using Observer.Common.Implementations.Fakes.Services;
using Observer.Common.Implementations.Fakes.Services.FakeContentProvider;
using Observer.Common.Implementations.Observation;
using Observer.Common.Implementations.Services;
using Observer.Common.Implementations.Services.Content;
using Observer.Common.Implementations.Services.Notifier;
using Observer.Common.Implementations.Services.Result;
using Observer.Common.Interfaces.Observation;
using Observer.Common.Interfaces.Services;
using Observer.EntityFramework;

namespace Observer.Common.Implementations.Fakes.Observation;

public sealed class FakeObservationFactoryBuilder
{
    private readonly BuildingSettings _settings = new();

    public void IntegrateDatabase() => _settings.IntegrateDatabase = true;
    public void NotIntegrateDatabase() => _settings.IntegrateDatabase = false;
    public void IntegrateObservationSubject() => _settings.IntegrateObservationSubject = true;
    public void NotIntegrateObservationSubject() => _settings.IntegrateObservationSubject = false;
    public void IntegrateNotifier() => _settings.IntegrateNotifier = true;
    public void NotIntegrateNotifier() => _settings.IntegrateNotifier = false;
    public void SetObservationSubjectStubType(ObservationSubjectStubType type)
        => _settings.ObservationSubjectStubType = type;

    public IObservationDispatcherFactory Build()
    {
        var services = new ServiceCollection();
        AddDatabase();
        AddObservationSubject();
        AddNotifier();
        AddCommonServices();
        return new FakeObservationDispatcherFactory(services.BuildServiceProvider());

        void AddDatabase()
        {
            if (_settings.IntegrateDatabase)
            {
                services.AddDbContext<AppDbContext>();
                services.AddTransient<IRecipientsProvider, RecipientsProvider>();
                services.AddTransient<IHistoryRepository, HistoryRepository>();
            }
            else
            {
                services.AddTransient<IRecipientsProvider, FakeRecipientsProvider>();
                services.AddTransient<IHistoryRepository, FakeHistoryRepository>();
            }
        }

        void AddObservationSubject()
        {
            if (_settings.IntegrateObservationSubject)
                services.AddTransient<IContentProvider, ContentProvider>();
            else
                ObservationSubjectStubFactory.AddObservationStubToServices(services,
                    _settings.ObservationSubjectStubType);
        }

        void AddNotifier()
        {
            if (_settings.IntegrateNotifier)
                services.AddTransient<INotifier, RabbitMqNotifier>();
            else
                services.AddTransient<INotifier, FakeNotifier>();
        }

        void AddCommonServices()
        {
            services.AddTransient<IObservationDispatcher, ObservationDispatcher>();
            services.AddTransient<IDataComparer, DataComparer>();
            services.AddTransient<INotifierTaskBuilder, NotifierTaskBuilder>();
            services.AddTransient<IResultHandler, ResultHandler>();

            services.AddLogging();
            services.AddTransient<IConfiguration>(static _ => CreateConfiguration());
        }
    }

    private static IConfiguration CreateConfiguration()
    {
        var config = new ConfigurationBuilder();
        config.AddInMemoryCollection(FakeConfigurationProvider.GetFakeConfiguration());
        return config.Build();
    }

    private sealed class BuildingSettings
    {
        public bool IntegrateDatabase { get; set; }
        public bool IntegrateObservationSubject { get; set; }
        public ObservationSubjectStubType ObservationSubjectStubType { get; set; }
        public bool IntegrateNotifier { get; set; }
    }

    private static class ObservationSubjectStubFactory
    {
        public static void AddObservationStubToServices(IServiceCollection services,
            ObservationSubjectStubType stubType)
        {
            switch (stubType)
            {
                case ObservationSubjectStubType.Volatile:
                    services.AddTransient<IContentProvider, FakeVolatileContentProvider>();
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
    
    public enum ObservationSubjectStubType
    {
        Volatile // Stub returns one state at the first time and another state at the second time.
    }
}
