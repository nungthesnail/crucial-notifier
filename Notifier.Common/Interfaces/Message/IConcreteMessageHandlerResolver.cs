namespace Notifier.Common.Interfaces.Message;

public interface IConcreteMessageHandlerResolver
{
    IMessageHandler Resolve(string jsonMessage);
}