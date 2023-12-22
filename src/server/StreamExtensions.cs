using System.Net.Sockets;

public static class StreamExtensions
{
    public static void WriteToStream(this NetworkStream stream, string message)
    {
        byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);

        stream.Write(msg, 0, msg.Length);
    }
}