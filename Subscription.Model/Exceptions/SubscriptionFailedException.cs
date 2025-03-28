namespace Subscription.Model.Exceptions;

public class SubscriptionFailedException(string? message = null, Exception? innerException = null)
    : Exception(message, innerException);
