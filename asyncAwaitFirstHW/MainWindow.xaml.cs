using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace asyncAwaitFirstHW
{
    public class Products
    {
        public int responseCode { get; set; }
        public List<Product> products { get; set; }
    }

    public class Product
    {
        public int id { get; set; }
        public string name { get; set; }
        public string price { get; set; }
        public string brand { get; set; }
        public Category category { get; set; }
    }

    public class Category
    {
        public Usertype usertype { get; set; }
        public string category { get; set; }
    }

    public class Usertype
    {
        public string usertype { get; set; }
    }

    public partial class MainWindow : Window
    {
        private const string apiUrl = "https://automationexercise.com/api/productsList";
        private List<Product> productList = new List<Product>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            await DownloadData();
            ProductsDataGrid.ItemsSource = productList;
        }

        private async Task DownloadData()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string json = await client.GetStringAsync(apiUrl);
                    Products products = JsonConvert.DeserializeObject<Products>(json);

                    productList.AddRange(products.products);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
    }
}
