using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

IPAddress IP = IPAddress.Parse("127.0.0.1");
int Port = 8080;
TcpListener listener = new TcpListener(IP, Port);

Console.WriteLine("Waiting for messages.");

try
{
    listener.Start();

    while (true)
    {
        TcpClient Client = listener.AcceptTcpClient();

        try
        {
            using NetworkStream stream = Client.GetStream();
            byte[] byteBuffer = new byte[1024];
            int byteCount = stream.Read(byteBuffer, 0, byteBuffer.Length);
            string Message = Encoding.UTF8.GetString(byteBuffer, 0, byteCount);
            Console.WriteLine("Message: " + Message);
        }
        catch (Exception ex) { Console.WriteLine("Exception: " + ex.Message); }
        finally { Client.Close(); }
    }
}
catch (Exception ex)
{
    Console.WriteLine("Exception: " + ex.Message);
}
finally
{
    listener.Stop();
}
