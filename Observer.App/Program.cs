using Serilog;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureHostConfiguration(static bld => bld
        .AddJsonFile("appsettings.json")
        .AddEnvironmentVariables())
    
    .ConfigureLogging(static (ctx, logging) =>
    {
        logging.ClearProviders();
        logging.AddSerilog(Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(ctx.Configuration)
            .Enrich.FromLogContext()
            .CreateLogger());
    });

var host = builder.Build();
host.Run();
