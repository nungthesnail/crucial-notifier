namespace Crucial.Core.Exceptions;

public class InternalCourseException(string? message = null, Exception? innerException = null)
    : Exception(message, innerException);
    