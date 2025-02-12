namespace Notifier.Common.Interfaces.Message;

public interface IMessageHandler
{
    Task Handle(string message);
}