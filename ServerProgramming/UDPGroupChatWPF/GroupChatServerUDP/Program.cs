using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDPServer
{
    public class GroupChatServer
    {
        private UdpClient Server;
        private IPEndPoint serverEndPoint;
        private Dictionary<IPEndPoint, string> ClientList;
        private List<string> ChatHistory;

        public GroupChatServer(int serverPort)
        {
            Server = new UdpClient(serverPort);
            serverEndPoint = new IPEndPoint(IPAddress.Any, serverPort);
            ClientList = new Dictionary<IPEndPoint, string>();
            ChatHistory = new List<string>();
        }

        public void Start()
        {
            Console.WriteLine("Server started.\n");

            while (true)
            {
                try
                {
                    byte[] data = Server.Receive(ref serverEndPoint);
                    string message = Encoding.UTF8.GetString(data);

                    if (message.Contains("has joined the chat."))
                    {
                        IPEndPoint clientEP = serverEndPoint;
                        string clientName = message.Substring(0, message.IndexOf(" has joined the chat."));
                        ClientList[clientEP] = clientName;
                        Console.WriteLine(clientName + " has joined the chat.");
                        ChatHistory.Add(clientName + " has joined the chat.");

                        string historyMessage = "chatHistory:" + string.Join("\r\n", ChatHistory);
                        byte[] historyData = Encoding.UTF8.GetBytes(historyMessage);
                        Server.Send(historyData, historyData.Length, clientEP);
                    }
                    else
                    {
                        string senderName = ClientList[serverEndPoint];
                        Console.WriteLine(senderName + ": " + message);
                        BroadcastMessage(senderName + ": " + message);
                        ChatHistory.Add(senderName + ": " + message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error occurred while receiving messages: " + ex.Message);
                }
            }
        }

        public void Stop()
        {
            SaveChatHistory();
            Server.Close();
        }

        private void SaveChatHistory()
        {
            File.WriteAllLines("chathistory.txt", ChatHistory);
        }

        private void BroadcastMessage(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            foreach (var clientEP in ClientList.Keys)
            {
                Server.Send(data, data.Length, clientEP);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            int Port = 12345;
            var Server = new GroupChatServer(Port);
            Server.Start();
        }
    }
}
