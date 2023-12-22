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

            if (bytesRead >= BufferSize) continue;
            
            var handler = CommandFactory.FetchHandler(buffer.ToString());

            handler?.Handle(stream, cache);

            buffer = new StringBuilder();
        }
    }
}