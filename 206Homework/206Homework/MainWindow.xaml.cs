using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Windows;
using System.Text.Json;
using Microsoft.Extensions.Options;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace _206Homework
{
    public partial class MainWindow : Window
    {

        public StoreDbContext StoreContext;

        public MainWindow()
        {
            InitializeComponent();
            StoreContext = new StoreDbContext();
            LoadProducts();
        }

        private void LoadProducts()
        {
            var products = StoreContext.Products.ToList();
            ElementsDataGrid.ItemsSource = products;
        }

        private void AddNewProductBtn_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTxtBox.Text;
            string price = PriceTxtBox.Text;

            StoreContext.Add(new Product { Name = name, Price = price });
            StoreContext.SaveChanges();

            LoadProducts();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var sortedProducts = StoreContext.Products.OrderBy(p => p.Name).ToList();
            ElementsDataGrid.ItemsSource = sortedProducts;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var sortedProducts = StoreContext.Products.OrderBy(p => p.Price).ToList();
            ElementsDataGrid.ItemsSource = sortedProducts;
        }

        private void LimitCountBtn_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(ElementsTextBox.Text, out int count))
            {
                var limitedProducts = StoreContext.Products.Take(count).ToList();
                ElementsDataGrid.ItemsSource = limitedProducts;
            }
        }
    }
}
