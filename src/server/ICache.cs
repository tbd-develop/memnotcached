namespace server;

public interface ICache
{
    void Add(string key, string data, TimeSpan? expiration = null);
    string? Get(string key);
    bool Delete(string key);
}