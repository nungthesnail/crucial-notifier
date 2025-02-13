using Microsoft.Extensions.Configuration;
using Notifier.Common.Implementations.BasicNotification;
using Tests.Common.Implementations;

namespace Notifier.Common.Tests.Integration;

public class EmailIntegrationTests
{
    [Test]
    public Task TestSendAsync()
    {
        const string messageTemplate = "Crucial Notifier: Email integration tests\n" +
                                   "Mail recipients: {0}\nSending time: {1}\n" +
                                   "Project info: https://github.com/nungthesnail/crucial-notifier";
        
        var config = BuildConfiguration();
        var logger = new LoggerStub<EmailBasicNotificationSender>();
        var sender = new EmailBasicNotificationSender(logger, config);
        var recipients = config
            .GetSection("EmailIntegrationTests:Recipients")
            .Get<string[]>() 
                         ?? throw new InvalidOperationException("В конфигурации не указан список получателей");
        var message = string.Format(
            messageTemplate,
            string.Join(", ", recipients),
            DateTimeOffset.Now);
        
        return sender.SendAsync(recipients, message);
    }

    private static IConfiguration BuildConfiguration()
    {
        var builder = new ConfigurationBuilder();
        builder.AddJsonFile("appsettings.development.json");
        return builder.Build();
    }
}
