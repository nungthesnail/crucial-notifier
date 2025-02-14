using Notifier.Common.Implementations;
using Notifier.Common.Implementations.BasicNotification;
using Notifier.Common.Implementations.Message;
using Notifier.Common.Interfaces.BasicNotification;
using Notifier.Common.Interfaces.Message;
using Notifier.Common.Models.BasicNotification;
using Notifier.Common.Models.Notification;
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

builder.ConfigureServices(static services =>
{
    services.AddHostedService<BrokerListener>();
    services.AddTransient<IMessageHandler, CommonMessageHandler>();
    services.AddTransient<IMessageDeserializer, MessageDeserializer>();
    services.AddTransient<IConcreteMessageHandlerResolver, ConcreteMessageHandlerResolver>();
    services.AddKeyedTransient<IMessageHandler, BasicNotificationHandler>(MessagesTypes.BasicNotification);
    services.AddTransient<IBasicNotificationSenderResolver, BasicNotificationSenderResolver>();
    services.AddKeyedTransient<IBasicNotificationSender, EmailBasicNotificationSender>(
        BasicNotificationSenderTypes.Email);
});

var host = builder.Build();
host.Run();
