using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.DependencyInjection;
using Notifier.Common.Exceptions;
using Notifier.Common.Interfaces.Message;

namespace Notifier.Common.Implementations.Message;

public class ConcreteMessageHandlerResolver : IConcreteMessageHandlerResolver
{
    private const string SchemaName = "nungthesnail@mail.ru:crucial-notifier-notification";
    private readonly IServiceProvider _serviceProvider;

    public ConcreteMessageHandlerResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public IMessageHandler Resolve(string jsonMessage)
    {
        try
        {
            var jsonNode = JsonNode.Parse(jsonMessage) ?? throw new BadJsonException("Can't parse json message");
            ThrowIfSchemaIsInvalid(jsonNode);
            var messageType = ExtractMessageType(jsonNode);
            return _serviceProvider.GetKeyedService<IMessageHandler>(messageType)
                   ?? throw new BadJsonException($"There is no message type \"{messageType}\"");
        }
        catch (JsonException exc)
        {
            throw new BadJsonException(exc.Message);
        }
    }
    
    private static void ThrowIfSchemaIsInvalid(JsonNode rootNode)
    {
        var schema = rootNode["Schema"] ?? throw new BadJsonException("Json schema isn't specified");
        var schemaValue = schema
            .AsValue()
            .GetValue<string>();
            
        if (!schemaValue.Equals(SchemaName, StringComparison.InvariantCultureIgnoreCase))
            throw new BadJsonException($"Invalid json schema value: {schemaValue}");
    }
    
    private static string ExtractMessageType(JsonNode rootNode)
    {
        var messageType = rootNode["MessageType"] ?? throw new BadJsonException("Message type isn't specified");
        return messageType
            .AsValue()
            .GetValue<string>();
    }
}
