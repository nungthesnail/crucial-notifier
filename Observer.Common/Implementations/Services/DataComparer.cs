using Observer.Common.Interfaces.Services;
using Observer.Common.Models;

namespace Observer.Common.Implementations.Services;

public class DataComparer : IDataComparer
{
    public ComparisonResult Compare(WebPageContent current, WebPageContent? previous)
    {
        ArgumentNullException.ThrowIfNull(current);
        if (previous is null)
            return new ComparisonResult
            {
                HashesIdentical = false,
                ModifiedTimestampsIdentical = false
            };

        return new ComparisonResult
        {
            HashesIdentical = current.Hash.Equals(previous.Hash),
            ModifiedTimestampsIdentical = current.LastModified == previous.LastModified,
            LastModifiedTimestamp = current.LastModified > previous.LastModified
                ? current.LastModified : previous.LastModified
        };
    }
}
