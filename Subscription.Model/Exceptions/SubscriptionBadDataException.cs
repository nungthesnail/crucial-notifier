namespace Subscription.Model.Exceptions;

public class SubscriptionBadDataException(string? message = null, Exception? innerException = null)
    : Exception(message, innerException);
