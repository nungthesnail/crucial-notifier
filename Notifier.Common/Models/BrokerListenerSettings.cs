namespace Notifier.Common.Models;

public class BrokerListenerSettings
{
    public string Host { get; set; } = string.Empty;
    public string Queue { get; set; } = string.Empty;
    
    public string User { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
