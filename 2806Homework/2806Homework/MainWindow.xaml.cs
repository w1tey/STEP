using System;
using System.Collections.Generic;
using System.Linq;
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

namespace _2806Homework
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> data = new List<string>
        {
            "Elvin",
            "Radjabos",
            "Orhanus",
            "Ilkin"
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(DownloadData);
            thread.Start();
        }

        private void DownloadData()
        {
            foreach (string item in data)
            {
                Thread.Sleep(100);

                Dispatcher.Invoke(() =>
                {
                    DataListBox.Items.Add(item);
                });
            }
        }
    }
}
