namespace Observer.Common.Exceptions;

public class ParsingException : Exception
{
    public ParsingException(string? message = null)
        : base(message)
    { }
}