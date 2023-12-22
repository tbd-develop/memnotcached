using System.Net.Sockets;
using server.Handlers;
using server.Infrastructure;

namespace server.Commands;

[Command("get")]
public class GetCommandHandler(CommandParameters parameters, string? data)
    : CommandHandler(parameters, data)
{
    public override void Handle(NetworkStream stream, Cache cache)
    {
        var value = cache.Get(Parameters.Key);

        if (value is not null)
        {
            stream.WriteToStream(
                $"VALUE {Parameters.Key} {value.Length}\r\n");
            stream.WriteToStream($"{value}\r\n");
            stream.WriteToStream("END\r\n");
        }
        else
        {
            stream.WriteToStream("END\r\n");
        }
    }
}