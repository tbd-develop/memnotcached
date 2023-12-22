using System.Net.Sockets;
using server.Handlers;
using server.Infrastructure;

namespace server.Commands;

[Command("set")]
public class SetCommandHandler(CommandParameters parameters, string data)
    : CommandHandler(parameters, data)
{
    public override void Handle(NetworkStream stream, Cache cache)
    {
        if (Data is null)
        {
            return;
        }

        cache.Add(Parameters.Key, Data,
            Parameters.ExpirationTimeSeconds > 0 ? TimeSpan.FromSeconds(Parameters.ExpirationTimeSeconds) : null);

        if (Parameters.Reply)
        {
            stream.WriteToStream("STORED\r\n");
        }
    }
}