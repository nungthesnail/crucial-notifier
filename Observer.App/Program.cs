using Microsoft.EntityFrameworkCore;
using Observer.Common.Implementations;
using Observer.Common.Implementations.Observation;
using Observer.Common.Implementations.Services;
using Observer.Common.Implementations.Services.Content;
using Observer.Common.Implementations.Services.Notifier;
using Observer.Common.Implementations.Services.Result;
using Observer.Common.Interfaces.Observation;
using Observer.Common.Interfaces.Services;
using Observer.EntityFramework;
using Serilog;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureHostConfiguration(static bld => bld
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables())
    .ConfigureLogging(static (ctx, logging) =>
    {
        logging.ClearProviders();
        logging.AddSerilog(Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(ctx.Configuration)
            .Enrich.FromLogContext()
            .CreateLogger());
    });

builder.ConfigureServices(static (hostContext, services) =>
{
    services.AddDbContext<AppDbContext>(options 
        => options.UseNpgsql(hostContext.Configuration.GetConnectionString("Postgres")));
    services.AddHostedService<WorkPlanner>();
    
    services.AddTransient<IRecipientsProvider, RecipientsProvider>();
    services.AddTransient<IHistoryRepository, HistoryRepository>();
    services.AddTransient<IContentProvider, ContentProvider>();
    services.AddTransient<INotifier, RabbitMqNotifier>();
    services.AddTransient<IObservationDispatcherFactory, ObservationDispatcherFactory>();
    services.AddTransient<IObservationDispatcher, ObservationDispatcher>();
    services.AddTransient<IDataComparer, DataComparer>();
    services.AddTransient<INotifierTaskBuilder, NotifierTaskBuilder>();
    services.AddTransient<IResultHandler, ResultHandler>();
    services.AddTransient<IHashCalculator, Md5HashCalculator>();
});

var host = builder.Build();
MigrateDatabase(host.Services);
host.Run();

return;

static void MigrateDatabase(IServiceProvider services)
{
    using var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}
