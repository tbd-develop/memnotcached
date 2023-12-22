using System.Net.Sockets;
using server.Handlers;
using server.Infrastructure;

namespace server.Commands;

[Command("delete")]
public class DeleteCommand(CommandParameters parameters, string? data)
    : CommandHandler(parameters, data)
{
    public override void Handle(NetworkStream stream, Cache cache)
    {
        var isDeleted = cache.Delete(parameters.Key);

        if (isDeleted && parameters.Reply)
            stream.WriteToStream("DELETED\r\n");
        else if (!isDeleted && parameters.Reply)
            stream.WriteToStream("NOT_FOUND\r\n");
    }
}