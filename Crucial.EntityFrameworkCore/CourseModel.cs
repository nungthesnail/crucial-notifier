using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crucial.EntityFrameworkCore;

[Table("course")]
public class CourseModel
{
    [Key]
    [Column("id")]
    [Required]
    public Guid Id { get; set; }
    
    [Column("name")]
    [Required]
    [MaxLength(128)]
    public required string Name { get; set; }
    
    [Column("description")]
    [MaxLength(512)]
    public string? Description { get; set; }
    
    [Column("total_lessons_count")]
    [Required]
    public int TotalLessonsCount { get; set; }
    
    [Column("lessons_passed_count")]
    [DefaultValue(0)]
    [Required]
    public int LessonsPassedCount { get; set; }
}
