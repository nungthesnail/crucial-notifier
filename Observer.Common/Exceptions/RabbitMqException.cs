namespace Observer.Common.Exceptions;

public class RabbitMqException : Exception
{
    public RabbitMqException(string? message = null)
        : base(message)
    { }
}