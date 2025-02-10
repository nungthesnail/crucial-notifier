namespace Observer.EntityFramework.Exceptions;

public class BadConfigurationException : Exception
{
    public BadConfigurationException(string? message = null)
        : base(message)
    { }
}