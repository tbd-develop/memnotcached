namespace server.tests;

public class when_command_is_simple
{
    private const string Command = "get";
    private const string Key = "key1";
    private const string SimpleCommand = $@"{Command} {Key}\r\n";
    private (Command, string?)? Result = default;

    public when_command_is_simple()
    {
        Arrange();

        Act();
    }

    [Fact]
    public void command_name_is_set()
    {
        Assert.Equal(Result.Value.Item1.Name, Command);
    }

    [Fact]
    public void key_is_set()
    {
        Assert.Equal(Result.Value.Item1.Key, Key) ;
    }

    private void Arrange()
    {
    }

    private void Act()
    {
        Result = CommandParser.Parse(SimpleCommand);
    }
}