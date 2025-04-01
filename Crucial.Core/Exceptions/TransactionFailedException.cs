namespace Crucial.Core.Exceptions;

public class TransactionFailedException(string? message = null, Exception? innerException = null)
    : TransactionException(message, innerException);
    