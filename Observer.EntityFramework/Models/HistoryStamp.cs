using System.ComponentModel.DataAnnotations;

namespace Observer.EntityFramework.Models;

public class HistoryStamp
{
    public int Id { get; set; }
    public DateTimeOffset? LastModified { get; set; }
    [MaxLength(64)] public string Hash { get; set; } = string.Empty;
    public DateTimeOffset InsertedAt { get; set; }
}