namespace Notifier.Common.Interfaces.Message;

public interface IMessageDeserializer
{
    TResult DeserializeMessage<TResult>(string message);
}