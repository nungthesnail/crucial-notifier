using System.ComponentModel.DataAnnotations;

namespace Observer.EntityFramework.Models;

public class Recipient
{
    public int Id { get; set; }
    public int Type { get; set; }
    [MaxLength(256)]
    public string? Identifier  { get; set; }
    public bool Active { get; set; }
}