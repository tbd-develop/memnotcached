// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.CommandLineUtils;

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

int MaximumBytes = 256;
Byte[] bytes = new Byte[MaximumBytes];
StringBuilder buffer = new StringBuilder();

Dictionary<string, string> _cache = new Dictionary<string, string>();

do
{
    using var client = await server.AcceptTcpClientAsync();

    var stream = client.GetStream();

    var bytesRead = 0;

    while ((bytesRead = stream.Read(bytes, 0, bytes.Length)) != 0)
    {
        buffer.Append(Encoding.ASCII.GetString(bytes, 0, bytesRead));

        if (bytesRead < MaximumBytes)
        {
            var result = CommandParser.Parse(buffer.ToString());

            if (result is not null)
            {
                switch (result.Value.command.Name)
                {
                    case "set":
                    {
                        _cache[result.Value.command.Key] = result.Value.data;

                        if (!result.Value.command.Noreply)
                        {
                            stream.WriteToStream("STORED\r\n");
                        }
                    }
                        break;
                    case "get":
                    {
                        if (_cache.TryGetValue(result.Value.command.Key, out var value))
                        {
                            stream.WriteToStream(
                                $"VALUE {result.Value.command.Key} {value.Length}\r\n");
                            stream.WriteToStream($"{value}\r\n");
                            stream.WriteToStream("END\r\n");
                        }
                        else
                        {
                            stream.WriteToStream("END\r\n");
                        }
                    }
                        break;
                }
            }

            buffer = new StringBuilder();
        }
    }
} while (true);