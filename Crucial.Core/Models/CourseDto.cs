namespace Crucial.Core.Models;

public class CourseDto
{
    public CourseDto(Guid id, string name, string? description, int totalLessonsCount, IEnumerable<LessonDto> lessons)
    {
        Id = id;
        Name = name;
        Description = description;
        TotalLessonsCount = totalLessonsCount;
        Lessons = lessons.ToList();
    }

    public CourseDto(Guid id, string name, string? description, int totalLessonsCount, int lessonsPassedCount = 0)
    {
        Id = id;
        Name = name;
        Description = description;
        TotalLessonsCount = totalLessonsCount;

        GenerateLessonsIfNotDefault(lessonsPassedCount);
    }

    private void GenerateLessonsIfNotDefault(int passedCount)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(passedCount, 0);
        if (passedCount == 0) return;

        foreach (var _ in Enumerable.Range(0, passedCount))
        {
            Lessons.Add(new LessonDto
            {
                Id = Guid.NewGuid(),
                Date = DateTime.Now
            });
        }
    }

    public Guid Id { get; init; }
    public string Name { get; init; }
    public string? Description { get; init; }
    public int TotalLessonsCount { get; }
    public int LessonsPassedCount => Lessons.Count;
    public List<LessonDto> Lessons { get; init; } = [];
}
