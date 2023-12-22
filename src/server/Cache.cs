using System.Collections.Concurrent;

namespace server;

public class Cache : ICache
{
    private readonly ConcurrentDictionary<string, CacheValue> _data = new();

    public void Add(string key, string data, TimeSpan? expiration = null)
    {
        _data[key] =
            new CacheValue(data,
                expiration is not null ? DateTime.UtcNow.Add(expiration.Value) : null);
    }

    public string? Get(string key)
    {
        var result = _data.GetValueOrDefault(key);

        if (result is null)
        {
            return null;
        }

        if (result.Expiration is null || !(result.Expiration < DateTime.UtcNow))
        {
            return result.Data;
        }

        _data.TryRemove(key, out _);

        return null;
    }

    public bool Delete(string key)
    {
        return _data.ContainsKey(key) && _data.TryRemove(key, out _);
    }
}

public record CacheValue(string Data, DateTime? Expiration);