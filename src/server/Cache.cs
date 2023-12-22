using System.Collections.Concurrent;

namespace server;

public class Cache
{
    private readonly ConcurrentDictionary<string, string> _data = new();

    public void Add(string key, string data)
    {
        _data[key] = data;
    }

    public string? Get(string key)
    {
        return _data.GetValueOrDefault(key);
    }
}