using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Notifier.Common.Implementations.BasicNotification;
using Notifier.Common.Implementations.Message;
using Notifier.Common.Interfaces.BasicNotification;
using Notifier.Common.Interfaces.Message;
using Notifier.Common.Models.BasicNotification;
using Notifier.Common.Models.Notification;

namespace Notifier.Common.Implementations.Fakes;

public sealed class FakeHostedServiceBuilder
{
    private readonly FakeMessageHandlerBuilderSettings _settings = new();

    public void IntegrateRabbitMq() => _settings.IntegrateRabbitMq = true;
    public void NotIntegrateRabbitMq() => _settings.IntegrateRabbitMq = false;
    public void IntegrateEmailSender() => _settings.IntegrateEmailSender = true;
    public void NotIntegrateEmailSender() => _settings.IntegrateEmailSender = false;

    public IHostedService Build()
    {
        var services = new ServiceCollection();

        AddRabbitMq();
        AddEmailSender();
        AddCommonServices();
        
        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider.GetService<IHostedService>()!;

        void AddRabbitMq()
        {
            if (_settings.IntegrateRabbitMq)
                services.AddSingleton<IHostedService, BrokerListener>();
            else
                services.AddSingleton<IHostedService, FakeBrokerListener>();
        }

        void AddEmailSender()
        {
            if (_settings.IntegrateEmailSender)
                services.AddKeyedTransient<IBasicNotificationSender, EmailBasicNotificationSender>(
                    BasicNotificationSenderTypes.Email);
            else
                services.AddKeyedTransient<IBasicNotificationSender, FakeEmailBasicNotificationSender>(
                    BasicNotificationSenderTypes.Email);
        }

        void AddCommonServices()
        {
            services.AddLogging(static builder => builder.AddConsole());

            services.AddTransient<IBasicNotificationSenderResolver, BasicNotificationSenderResolver>();
            services.AddTransient<IConcreteMessageHandlerResolver, ConcreteMessageHandlerResolver>();
            services.AddTransient<IMessageDeserializer, MessageDeserializer>();
            services.AddTransient<IMessageHandler, CommonMessageHandler>();
            services.AddKeyedTransient<IMessageHandler, BasicNotificationHandler>(MessagesTypes.BasicNotification);
        }
    }
    
    private sealed class FakeMessageHandlerBuilderSettings
    {
        public bool IntegrateRabbitMq { get; set; }
        public bool IntegrateEmailSender { get; set; }
    }
}
