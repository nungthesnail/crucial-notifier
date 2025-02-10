namespace Observer.Common.Exceptions;

public class HttpFailedRequestException : Exception
{
    public HttpFailedRequestException(string? message = null)
        : base(message)
    { }
}