using System.Net.Sockets;
using server.Handlers;

namespace server.Commands;

public abstract class CommandHandler(CommandParameters parameters, string? data)
{
    protected readonly CommandParameters Parameters = parameters;
    protected readonly string? Data = data;

    public abstract void Handle(NetworkStream stream, Cache cache);
}