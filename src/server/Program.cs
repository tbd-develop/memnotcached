// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Net.Sockets;
using server;

int port = 11211;

if (args.Length > 0)
{
    if (!int.TryParse(args[0], out port))
    {
        Console.WriteLine("Invalid port number");

        return;
    }
}

var server = new TcpListener(IPAddress.Loopback, port);

server.Start();

var cache = new Cache();

var taskPool = new List<Task>();

do
{
    var client = await server.AcceptTcpClientAsync();

    var clientHandler = new NotCachedClient(cache, client);

    taskPool.Add(Task.Factory.StartNew(async () => await clientHandler.ListenForMessages(),
        TaskCreationOptions.LongRunning));
} while (true);