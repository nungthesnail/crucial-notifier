namespace Observer.Common.Models;

public class WebPageContent
{
    public string Hash { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTimeOffset? LastModified { get; set; }
}
