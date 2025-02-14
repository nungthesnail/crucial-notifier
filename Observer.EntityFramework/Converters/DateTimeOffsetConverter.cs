using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Observer.EntityFramework.Converters;

public class DateTimeOffsetConverter : ValueConverter<DateTimeOffset, DateTimeOffset>
{
    public DateTimeOffsetConverter()
        : base(
            static x => x.ToUniversalTime(),
            static x => x.ToUniversalTime())
    { }
}