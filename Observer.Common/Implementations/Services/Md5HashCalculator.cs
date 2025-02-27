using System.Security.Cryptography;
using Observer.Common.Interfaces.Services;

namespace Observer.Common.Implementations.Services;

public class Md5HashCalculator : IHashCalculator
{
    public async Task<string> CalculateHashAsync(string content)
    {
        ArgumentNullException.ThrowIfNull(content);
        
        var streamContent = GenerateStreamFromString(content);
        var hasher = MD5.Create();
        var hash = await hasher.ComputeHashAsync(streamContent);
        return BitConverter
            .ToString(hash)
            .Replace("-", string.Empty)
            .ToLowerInvariant();
    }
    
    private static MemoryStream GenerateStreamFromString(string s)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }
}
