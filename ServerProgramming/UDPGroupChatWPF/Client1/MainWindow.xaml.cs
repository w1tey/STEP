using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace Client1
{
    public partial class MainWindow : Window
    {
        private UdpClient client;
        private IPEndPoint serverEndpoint;
        private bool isConnected = false;
        private string clientName;
        private bool isNameSent = false;
        private ObservableCollection<string> chatMessages = new ObservableCollection<string>();

        public MainWindow()
        {
            InitializeComponent();

            client = new UdpClient();
            serverEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);

            ChatHistory.DataContext = chatMessages;
        }

        private void Connect(string clientName)
        {
            this.clientName = clientName;
            try
            {
                byte[] joinData = Encoding.UTF8.GetBytes(this.clientName + " has joined the chat.");
                client.Send(joinData, joinData.Length, serverEndpoint);

                Thread receiveThread = new Thread(ReceiveMessages);
                receiveThread.Start();

                isConnected = true;

                MessageBox.Show("Name successfully allocated: " + this.clientName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while connecting or sending name: " + ex.Message);
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

                    if (message.StartsWith("chatHistory:"))
                    {
                        string[] messages = message.Substring(12).Split(new string[] { "\r\n" }, StringSplitOptions.None);

                        Dispatcher.Invoke(() =>
                        {
                            chatMessages.Clear();
                            foreach (string msg in messages)
                            {
                                chatMessages.Add(msg);
                            }
                        });
                    }
                    else
                    {
                        Dispatcher.Invoke(() =>
                        {
                            chatMessages.Add(message);
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while receiving messages: " + ex.Message);
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isNameSent)
            {
                clientName = MessageTextBox.Text;
                Connect(clientName);
                isNameSent = true;
                MessageTextBox.Clear();
            }
            else
            {
                if (isConnected)
                {
                    string message = MessageTextBox.Text;
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    client.Send(data, data.Length, serverEndpoint);
                    MessageTextBox.Clear();
                }
            }
        }
    }
}
