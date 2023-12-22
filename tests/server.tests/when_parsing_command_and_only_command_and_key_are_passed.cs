using server.Handlers;

namespace server.tests;

public class when_parsing_command_and_only_command_and_key_are_passed
{
    private const string Command = "get";
    private const string ExpectedKey = "key1";
    private const string SimpleCommand = $@"{Command} {ExpectedKey}\r\n";
    private (CommandParameters parameters, string? data)? Result = default;

    public when_parsing_command_and_only_command_and_key_are_passed()
    {
        Arrange();

        Act();
    }

    [Fact]
    public void key_is_set()
    {
        Assert.Equal(ExpectedKey, Result.Value.parameters.Key);
    }

    private void Arrange()
    { }

    private void Act()
    {
        Result = CommandParametersParser.Parse(SimpleCommand);
    }
}