namespace Crucial.Core.Exceptions;

public class CourseDoesntExistsException(string? message = null, Exception? innerException = null)
    : BadDataProvidedException(message, innerException);
