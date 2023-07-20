using System;
using System.Net.Sockets;
using System.Text;

try
{
    TcpClient Client = new TcpClient();
    Client.Connect("127.0.0.1", 8080);
    Console.WriteLine("Connection Successful");

    while (true)
    {
        Console.Write("Enter a message to send.\nEnter 0 to exit.\nEnter: ");
        string Message = Console.ReadLine();

        byte[] byteMessage = Encoding.UTF8.GetBytes(Message);
        NetworkStream Stream = Client.GetStream();
        Stream.Write(byteMessage, 0, byteMessage.Length);

        if (Message == "0")
        {
            Client.Close();
            break;
        }
    }
}
catch (Exception ex) { Console.WriteLine("Exception: " + ex.Message); }
