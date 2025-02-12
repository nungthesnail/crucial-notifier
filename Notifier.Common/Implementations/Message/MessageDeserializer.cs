using System.Text.Json;
using Notifier.Common.Interfaces.Message;

namespace Notifier.Common.Implementations.Message;

public class MessageDeserializer : IMessageDeserializer
{
    public TResult DeserializeMessage<TResult>(string message)
    {
        return JsonSerializer.Deserialize<TResult>(message)
               ?? throw new FormatException($"Can't deserialize message to type {typeof(TResult).Name}");
    }
}
