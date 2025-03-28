namespace Crucial.Core.Models;

public record CourseDto(
    Guid CourseId,
    string Name,
    string? Description,
    int TotalLessonsCount,
    int LessonsPassedCount = 0);
