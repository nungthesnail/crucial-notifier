namespace Observer.Common.Exceptions;

public class BadTaskException : Exception
{
    public BadTaskException(string? message = null)
        : base(message)
    { }
}