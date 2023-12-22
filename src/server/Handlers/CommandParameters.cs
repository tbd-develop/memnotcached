namespace server.Handlers;

public class CommandParameters(string key)
{
    public string Key { get; private set; } = key;
    public int? Flags { get; private set; }
    public int ExpirationTimeSeconds { get; private set; } = 0;
    public int? DataSize { get; private set; }
    public bool Reply { get; private set; } = true;

    public CommandParameters(string key, int flags, int expirationTimeSeconds, int dataSize, bool reply)
        : this(key)
    {
        Flags = flags;
        ExpirationTimeSeconds = expirationTimeSeconds;
        DataSize = dataSize;
        Reply = reply;
    }
}