namespace Brokers.Common;

public class MessageReceivedEventArgs : EventArgs
{
    public string? Message { get; set; }
}
