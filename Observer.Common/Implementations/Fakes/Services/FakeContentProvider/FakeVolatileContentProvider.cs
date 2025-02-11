using Observer.Common.Interfaces.Services;
using Observer.Common.Models;

namespace Observer.Common.Implementations.Fakes.Services.FakeContentProvider;

public class FakeVolatileContentProvider : IContentProvider
{
    private readonly WebPageContent[] _contents = [
        new()
        {
            Hash = "b3c731af00579bfe16b1ffeb7d9eb40e",
            LastModified = DateTimeOffset.Parse("2021-09-17")
        },
        new()
        {
            Hash = "d552920da3a383a69bca19a4fbd1f54b",
            LastModified = DateTimeOffset.Parse("2022-09-17")
        }
    ];
    private int _counter;
    
    public Task<WebPageContent> GetContentAsync(CancellationToken _)
    {
        return Task.FromResult(_contents[_counter++ % 2]);
    }
}