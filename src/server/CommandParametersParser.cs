using System.Text.RegularExpressions;
using server.Handlers;

namespace server;

public class CommandParametersParser
{
    private static string CommandPattern = @"^(?<command>[\w\d]*)\s(?<key>[\w\d]*)";

    private static string FullPattern =
        "^(?<command>[\\w\\d]*)\\s(?<key>[\\w\\d]*)\\s(?<flags>[\\d]*){0,1}\\s(?<exptime>[\\d]*){0,1}\\s(?<bytes>[\\d]*){0,1}\\s{0,1}(?<noreply>\\[noreply\\]){0,1}";

    public static (CommandParameters parameters, string? data)? Parse(string input)
    {
        input = Regex.Unescape(input);

        if (!input.Contains(Environment.NewLine))
        {
            return null;
        }

        var parameters = input.Split(Environment.NewLine);
        var commandString = parameters[0];
        var data = parameters[1];

        var matches = Regex.Match(commandString, CommandPattern, RegexOptions.Multiline);

        if (!matches.Success) return null;

        var fullMatches = Regex.Match(commandString, FullPattern, RegexOptions.Multiline);

        if (!fullMatches.Success)
        {
            return (new CommandParameters(matches.Groups["key"].Value.ToLower()), data);
        }

        return new(
            new CommandParameters(
                fullMatches.Groups["key"].Value.ToLower(),
                fullMatches.Groups["flags"].Value.ConvertToInt(),
                fullMatches.Groups["exptime"].Value.ConvertToInt(),
                fullMatches.Groups["bytes"].Value.ConvertToInt(),
                fullMatches.Groups["noreply"].Value.ToLower() != "[noreply]")
            , data);
    }
}