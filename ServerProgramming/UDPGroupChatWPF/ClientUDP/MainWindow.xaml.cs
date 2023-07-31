using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientUDP
{
    public class GroupChatClient
    {
        private UdpClient client;
        private IPEndPoint serverEndpoint;
        private bool isConnected = false;
        private string name;

        public GroupChatClient(string serverIPAddress, int serverPort)
        {
            client = new UdpClient();
            serverEndpoint = new IPEndPoint(IPAddress.Parse(serverIPAddress), serverPort);
        }

        public void Connect(string clientName)
        {
            name = clientName;
            try
            {
                byte[] joinData = Encoding.UTF8.GetBytes(name + " has joined the chat.");
                client.Send(joinData, joinData.Length, serverEndpoint);

                Thread receiveThread = new Thread(ReceiveMessages);
                receiveThread.Start();

                isConnected = true;

                while (true)
                {
                    if (isConnected)
                    {
                        string message = Console.ReadLine();
                        byte[] data = Encoding.UTF8.GetBytes(message);
                        client.Send(data, data.Length, serverEndpoint);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred while connecting or sending name: " + ex.Message);
            }
        }

        private void ReceiveMessages()
        {
            try
            {
                IPEndPoint serverEndpoint = new IPEndPoint(IPAddress.Any, 0);
                while (true)
                {
                    byte[] data = client.Receive(ref serverEndpoint);
                    string message = Encoding.UTF8.GetString(data);
                    Console.WriteLine(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred while receiving messages: " + ex.Message);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter your name:");
            string name = Console.ReadLine();

            var client = new GroupChatClient("127.0.0.1", 12345);
            client.Connect(name);
        }
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
