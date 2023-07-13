using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace asyncAwaitSecondHW
{
    public partial class MainWindow : Window
    {
        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Price { get; set; }
        }

        public class StoreDbContext : DbContext
        {
            public DbSet<Product> Products { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                string connectionString = configuration.GetConnectionString("LocalConnection");

                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        public StoreDbContext StoreContext;

        public MainWindow()
        {
            InitializeComponent();
            StoreContext = new StoreDbContext();
        }

        private async void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new StoreDbContext())
            {
                db.Products.Add(new Product { Name = NameAddTxtBox.Text, Price = PriceAddTxtBox.Text });
                await db.SaveChangesAsync();
            }

            ReturnAllBtn_Click(sender, e);

            NameAddTxtBox.Text = "";
            PriceAddTxtBox.Text = "";
        }

        private async void ReadBtn_Click(object sender, RoutedEventArgs e)
        {
            string searchName = EnterNameTxtBox.Text;

            using (var context = new StoreDbContext())
            {
                var products = await context.Products
                    .Where(p => p.Name.Contains(searchName))
                    .ToListAsync();

                if (products.Count == 0)
                {
                    MessageBox.Show("No matching items found.");
                }
                else
                {
                    ProductDataGrid.ItemsSource = products;
                }
            }

            ReturnAllBtn_Click(sender, e);

            EnterNameTxtBox.Text = "";
        }

        private async void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(IdUpdateTxtBox.Text, out int productId))
            {
                using (var db = new StoreDbContext())
                {
                    var product = await db.Products.FindAsync(productId);
                    if (product == null)
                    {
                        MessageBox.Show("Product not found");
                        return;
                    }

                    product.Name = NameUpdateTxtBox.Text;
                    product.Price = PriceUpdateTxtBox.Text;

                    db.Products.Update(product);
                    await db.SaveChangesAsync();
                }
            }
            else
            {
                MessageBox.Show("Invalid product ID");
            }

            ReturnAllBtn_Click(sender, e);

            IdUpdateTxtBox.Text = "";
            NameUpdateTxtBox.Text = "";
            PriceUpdateTxtBox.Text = "";
        }

        private async void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(DeleteIDTxtBox.Text, out int productId))
            {
                using (var db = new StoreDbContext())
                {
                    var product = await db.Products.FindAsync(productId);
                    if (product == null)
                        return;

                    db.Products.Remove(product);
                    await db.SaveChangesAsync();
                }
            }
            else
            {
                MessageBox.Show("Invalid product ID");
            }

            ReturnAllBtn_Click(sender, e);

            DeleteIDTxtBox.Text = "";
            NameDeleteTxtBox.Text = "";
            PriceDeleteTxtBox.Text = "";
        }

        private async void ReturnAllBtn_Click(object sender, RoutedEventArgs e)
        {
            var products = await StoreContext.Products.ToListAsync();
            ProductDataGrid.ItemsSource = products;
        }

    }
}
