namespace Crucial.Core.Exceptions;

public class CourseAlreadyExistsException(string? message = null, Exception? innerException = null)
    : BadDataProvidedException(message, innerException);
