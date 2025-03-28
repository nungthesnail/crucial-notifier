namespace Crucial.Core.Exceptions;

public class BadDataProvidedException(string? message = null, Exception? innerException = null)
    : Exception(message, innerException);
