namespace Crucial.Core.Models;

public record CourseDto(
    Guid Id,
    string Name,
    string? Description,
    int TotalLessonsCount,
    int LessonsPassedCount = 0);
