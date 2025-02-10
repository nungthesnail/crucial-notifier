using Observer.Common.Models;

namespace Observer.Common.Interfaces.Services;

public interface IResultHandler
{
    Task HandleResultAsync(ComparisonResult result);
}