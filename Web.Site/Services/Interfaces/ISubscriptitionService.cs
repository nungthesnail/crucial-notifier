namespace Web.Site.Services.Interfaces;

public interface ISubscriptitionService
{
    Task<bool> IsUserSubscribedAsync(string? userName);
}
