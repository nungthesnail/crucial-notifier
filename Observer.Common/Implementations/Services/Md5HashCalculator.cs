using System.Security.Cryptography;
using Observer.Common.Interfaces.Services;

namespace Observer.Common.Implementations.Services;

public class Md5HashCalculator : IHashCalculator
{
    public async Task<string> CalculateHashAsync(Stream content)
    {
        var hasher = MD5.Create();
        var hash = await hasher.ComputeHashAsync(content);
        return BitConverter
            .ToString(hash)
            .Replace("-", string.Empty)
            .ToLowerInvariant();
    }
}