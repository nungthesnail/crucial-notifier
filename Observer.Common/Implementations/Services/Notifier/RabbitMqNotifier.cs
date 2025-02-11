using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Observer.Common.Exceptions;
using Observer.Common.Interfaces.Services;
using Observer.Common.Models.Notifier;

namespace Observer.Common.Implementations.Services.Notifier;

public class RabbitMqNotifier : INotifier
{
    private readonly ILogger<RabbitMqNotifier> _logger;
    private readonly RabbitMqSender _sender;
    
    public RabbitMqNotifier(ILogger<RabbitMqNotifier> logger, IConfiguration config)
    {
        _logger = logger;
        _sender = new RabbitMqSender(config);
    }
    
    public Task SendAsync(NotifierTask task, CancellationToken stoppingToken)
    {
        _logger.LogInformation("Processing notification task...");
        ThrowIfTaskDataIsWrong();
        LogTaskInformation(task);
        
        var message = NotificationMessageBuilder.Build(task);
        var serializedMessage = JsonSerializer.Serialize(message);
        var result = _sender.SendMessageAsync(serializedMessage, stoppingToken);
        
        _logger.LogInformation("Notification task sent to message broker.");
        
        return result;
        
        void ThrowIfTaskDataIsWrong()
        {
            if (task.Data is null)
                throw new BadTaskException("Task data is null");
            
            if (!task.Data.HashUpdated && !task.Data.ModifiedTimestampUpdated)
                throw new BadTaskException("Hash and timestamp is not updated");
        }
    }

    private void LogTaskInformation(NotifierTask task)
    {
        _logger.LogInformation(
            "Notification data: Hash was {hashUpdated}, Last modified timestamp was {modifiedTimestampUpdated}",
            task.Data!.HashUpdated ? "updated" : "not updated",
            task.Data!.ModifiedTimestampUpdated ? "updated" : "not updated");
        
        _logger.LogInformation("Recipients:");
        foreach (var recipient in task.Recipients)
        {
            _logger.LogInformation("* {type}: {identifier}", recipient.Type, recipient.Identifier);
        }
    }
}