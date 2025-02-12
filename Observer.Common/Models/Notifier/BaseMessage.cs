namespace Observer.Common.Models.Notifier;

public abstract class BaseMessage
{
    public string Schema => "nungthesnail@mail.ru:crucial-notifier-notification";
    public abstract string MessageType { get; }
}