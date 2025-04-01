namespace Crucial.Core.Exceptions;

public class TransactionNotStartedException(string? message = null, Exception? innerException = null)
    : TransactionException(message, innerException);
