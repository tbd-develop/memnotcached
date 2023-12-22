using System.Reflection;
using System.Text.RegularExpressions;
using server.Commands;
using server.Infrastructure;

namespace server;

public static class CommandFactory
{
    private static readonly string CommandPattern = @"^(?<command>[\w\d]*)\s(?<key>[\w\d]*)";

    private static Dictionary<string, Type>? _availableCommands = null;

    public static CommandHandler? FetchHandler(string input)
    {
        input = Regex.Unescape(input);

        if (!input.Contains(Environment.NewLine))
        {
            return null;
        }

        var matches = Regex.Match(input, CommandPattern, RegexOptions.Multiline);

        if (!matches.Success) return null;

        LoadCommands();

        var commandType =
            _availableCommands!.GetValueOrDefault(matches.Groups["command"].Value.ToLower());

        if (commandType is null)
        {
            return null;
        }

        var result = CommandParametersParser.Parse(input);

        if (!result.HasValue)
        {
            return null;
        }

        return (CommandHandler)Activator.CreateInstance(commandType, [result.Value.parameters, result.Value.data])!;
    }

    private static void LoadCommands()
    {
        if (_availableCommands is not null) return;

        _availableCommands = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsSubclassOf(typeof(CommandHandler)) &&
                        !t.IsAbstract && t.GetCustomAttributes<CommandAttribute>().Any())
            .Select(t => new
            {
                Key = t.GetCustomAttribute<CommandAttribute>()!.Name,
                Value = t
            }).ToDictionary(k => k.Key, k => k.Value);
    }
}