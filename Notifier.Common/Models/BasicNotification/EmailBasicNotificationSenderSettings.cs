namespace Notifier.Common.Models.BasicNotification;

public class EmailBasicNotificationSenderSettings
{
    public string SmtpServerAddress { get; set; } = string.Empty;
    public int SmtpServerPort { get; set; } = 465;
    public string UserAddress { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string SenderName { get; set; } = string.Empty;
}