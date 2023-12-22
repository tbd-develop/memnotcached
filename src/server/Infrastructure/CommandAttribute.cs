namespace server.Infrastructure;

public class CommandAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}