namespace Subscription.Model.Exceptions;

/// <summary>
/// Used when data saved successfully but transaction hasn't been begun.
/// </summary>
public class TransactionNotStartedException(string? message = null, Exception? innerException = null)
    : TransactionException(message, innerException);
