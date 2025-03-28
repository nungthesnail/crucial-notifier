namespace Subscription.Model.Exceptions;

public class TransactionException(string? message = null, Exception? innerException = null)
    : Exception(message, innerException);
