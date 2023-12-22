using System.Net.Sockets;
using System.Text;

namespace server;

public class NotCachedClient(Cache cache, TcpClient client)
{
    private const int BufferSize = 256;

    public async Task ListenForMessages(CancellationToken cancellationToken = default)
    {
        var bytes = new byte[BufferSize];
        var buffer = new StringBuilder();

        var stream = client.GetStream();

        var bytesRead = 0;

        while ((bytesRead = await stream.ReadAsync(bytes, 0, bytes.Length, cancellationToken)) != 0)
        {
            buffer.Append(Encoding.ASCII.GetString(bytes, 0, bytesRead));

            if (bytesRead < BufferSize)
            {
                var result = CommandParser.Parse(buffer.ToString());

                if (result is not null)
                {
                    switch (result.Value.command.Name)
                    {
                        case "set":
                        {
                            if (result.Value.data is not null)
                            {
                                cache.Add(result.Value.command.Key, result.Value.data);

                                if (!result.Value.command.Noreply)
                                {
                                    stream.WriteToStream("STORED\r\n");
                                }
                            }
                        }
                            break;
                        case "get":
                        {
                            var value = cache.Get(result.Value.command.Key);

                            if (value is not null)
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
    }
}