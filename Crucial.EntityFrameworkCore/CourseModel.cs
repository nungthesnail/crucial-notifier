using System.ComponentModel.DataAnnotations;

namespace Crucial.EntityFrameworkCore;

public class CourseModel
{
    public Guid Id { get; set; }
    [MaxLength(128)]
    public required string Name { get; set; }
    [MaxLength(512)]
    public string? Description { get; set; }
    public int TotalLessonsCount { get; set; }
    public int LessonsPassedCount { get; set; }
}
