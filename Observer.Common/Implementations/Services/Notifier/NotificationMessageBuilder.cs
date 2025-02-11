using Observer.Common.Exceptions;
using Observer.Common.Models.Notifier;

namespace Observer.Common.Implementations.Services.Notifier;

public static class NotificationMessageBuilder
{
    public static NotificationMessage Build(NotifierTask notifierTask)
    {
        var content = BuildTextContent(notifierTask.Data);
        var recipients = BuildRecipients(notifierTask);
        return new NotificationMessage
        {
            Content = content,
            Recipients = recipients
        };
    }

    private static string BuildTextContent(NotificationData? data)
    {
        return data switch
        {
            {HashUpdated: true, ModifiedTimestampUpdated: true} => HashAndTimestamp(data.ModifiedTimestamp),
            {HashUpdated: false, ModifiedTimestampUpdated: true} => OnlyTimestamp(data.ModifiedTimestamp),
            {HashUpdated: true, ModifiedTimestampUpdated: false} => OnlyHash(),
            _ => throw new BadTaskException("Can't build message for wrong notification data")
        };
    }

    private static string HashAndTimestamp(DateTimeOffset? timestamp)
        => $"CrucialNotifier: Расписание было обновлено в {FormatTimestamp(timestamp)}. Обновлена временная метка и " +
           $"хеш страницы";
    
    private static string FormatTimestamp(DateTimeOffset? timestamp)
        => timestamp is not null 
            ? $"{timestamp.Value.ToLocalTime():yyyy-MM-dd HH:mm:ss}" 
            : "[временная метка отсутствует]";
    
    private static string OnlyTimestamp(DateTimeOffset? timestamp) // Will it ever happen?
        => "CrucialNotifier. Нестандартная ситуация: Временная метка на странице была обновлена, но хеш страницы " +
           $"остался прежним. Временная метка: {FormatTimestamp(timestamp)}";
    private static string OnlyHash()
        => "CrucialNotifier. Нестандартная ситуация: страница была обновлена, но временная " +
           "метка осталась прежней. Различие в хешах версий страницы.";

    private static IEnumerable<NotificationRecipient> BuildRecipients(NotifierTask notifierTask)
        => notifierTask.Recipients;
}