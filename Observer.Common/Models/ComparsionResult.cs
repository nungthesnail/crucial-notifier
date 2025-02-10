namespace Observer.Common.Models;

public class ComparisonResult
{
    public bool HashesIdentical { get; set; }
    public bool ModifiedTimestampsIdentical { get; set; }
    public DateTimeOffset? LastModifiedTimestamp { get; set; }
}
