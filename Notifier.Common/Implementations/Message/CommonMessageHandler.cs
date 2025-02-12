using Microsoft.Extensions.Logging;
using Notifier.Common.Interfaces.Message;

namespace Notifier.Common.Implementations.Message;

public class CommonMessageHandler : IMessageHandler
{
    private readonly ILogger<CommonMessageHandler> _logger;
    private readonly IConcreteMessageHandlerResolver _handlerResolver;

    public CommonMessageHandler(ILogger<CommonMessageHandler> logger, IConcreteMessageHandlerResolver handlerResolver)
    {
        _logger = logger;
        _handlerResolver = handlerResolver;
    }
    
    public async Task Handle(string message)
    {
        try
        {
            _logger.LogInformation("Message received. Handling it...");
            _logger.LogInformation("Resolving concrete message handler...");
            var messageHandler = _handlerResolver.Resolve(message);
            _logger.LogInformation("{resolver} will handle this message further.", messageHandler.GetType().Name);
            await messageHandler.Handle(message);
        }
        catch (Exception exc)
        {
            _logger.LogError(exc, "Something failed while handling message");
        }
    }
}
