using System.Text.RegularExpressions;

public static class CommandParser
{
    private static string CommandPattern = "^(?<command>[\\w\\d]*)\\s(?<key>[\\w\\d]*)";

    private static string FullPattern =
        "^(?<command>[\\w\\d]*)\\s(?<key>[\\w\\d]*)\\s(?<flags>[\\d]*){0,1}\\s(?<exptime>[\\d]*){0,1}\\s(?<bytes>[\\d]*){0,1}\\s{0,1}(?<noreply>\\[noreply\\]){0,1}";

    public static (Command command, string? data)? Parse(string input)
    {
        input = Regex.Unescape(input);

        if (!input.Contains(Environment.NewLine))
        {
            return null;
        }

        var components = input.Split("\r\n");
        var header = components[0];
        var data = components[1];

        var matches = Regex.Match(header, CommandPattern, RegexOptions.Multiline);

        if (!matches.Success) return null;

        if (matches.Groups["command"].Value == "set")
        {
            var fullMatches = Regex.Match(header, FullPattern, RegexOptions.Multiline);

            if (fullMatches.Success)
            {
                var command = new Command(
                    fullMatches.Groups["command"].Value.ToLower(),
                    fullMatches.Groups["key"].Value,
                    fullMatches.Groups["flags"].Value,
                    fullMatches.Groups["exptime"].Value,
                    fullMatches.Groups["bytes"].Value,
                    fullMatches.Groups["noreply"].Value.ToLower() == "[noreply]");

                return (command, data);
            }
        }
        else
        {
            var command = new Command(
                matches.Groups["command"].Value.ToLower(),
                matches.Groups["key"].Value
            );

            return (command, null);
        }

        return null;
    }
}