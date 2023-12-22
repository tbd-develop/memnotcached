namespace server.tests;

public class when_parsing_command_and_parameters_are_passed
{
    private const string Command = "get";
    private const string ExpectedKey = "key1";
    private const int ExpectedFlags = 1;
    private const string FlagsString = "1";
    private const int ExpectedExpirationTimeSeconds = 2;
    private const string ExpectedExpirationString = "2";
    private const int ExpectedDataSize = 3;
    private const string ExpectedDataSizeString = "3";
    private const bool ExpectedReply = false;

    private const string SimpleCommand =
        $"{Command} {ExpectedKey} {FlagsString} {ExpectedExpirationString} {ExpectedDataSizeString} [noreply]\r\n";

    private (CommandParameters parameters, string? data)? Result = default;

    public when_parsing_command_and_parameters_are_passed()
    {
        Arrange();

        Act();
    }

    [Fact]
    public void key_is_set()
    {
        Assert.Equal(ExpectedKey, Result.Value.parameters.Key);
    }

    [Fact]
    public void flags_are_set()
    {
        Assert.Equal(ExpectedFlags, Result.Value.parameters.Flags); // Assert
    }

    [Fact]
    public void expiration_time_is_set()
    {
        Assert.Equal(ExpectedExpirationTimeSeconds, Result.Value.parameters.ExpirationTimeSeconds); // Assert
    }

    [Fact]
    public void data_size_is_set()
    {
        Assert.Equal(ExpectedDataSize, Result.Value.parameters.DataSize); // Assert
    }

    private void Arrange()
    {
    }

    private void Act()
    {
        Result = CommandParametersParser.Parse(SimpleCommand);
    }
}