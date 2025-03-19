namespace Subscription.Model;

public class SubscriptionModel
{
    public Guid Id { get; set; }
    public required string UserId { get; set; }
    public bool Active { get; set; }
    public required string Email { get; set; }
}
