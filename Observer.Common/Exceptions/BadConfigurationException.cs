namespace Observer.Common.Exceptions;

public class BadConfigurationException : Exception
{
    public BadConfigurationException(string? message = null)
        : base(message)
    { }
}