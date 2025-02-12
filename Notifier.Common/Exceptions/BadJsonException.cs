namespace Notifier.Common.Exceptions;

public class BadJsonException : Exception
{
    public BadJsonException(string? message = null)
        : base(message)
    { }
}