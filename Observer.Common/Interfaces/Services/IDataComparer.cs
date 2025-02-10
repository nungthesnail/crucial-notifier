using Observer.Common.Models;

namespace Observer.Common.Interfaces.Services;

public interface IDataComparer
{
    ComparisonResult Compare(WebPageContent current, WebPageContent? previous);
}
