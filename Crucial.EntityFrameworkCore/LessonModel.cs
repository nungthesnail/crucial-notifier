using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crucial.EntityFrameworkCore;

public class LessonModel
{
    [Key]
    [Column("id")]
    [Required]
    public Guid Id { get; set; }
    
    [Column("date")]
    [Required]
    public DateTimeOffset Date { get; set; }
}
