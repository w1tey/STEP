using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace _706Homework
{
    public partial class MainWindow : Window
    {
        private readonly ProductDbContext ProductsContext;

        public MainWindow()
        {
            InitializeComponent();
            ProductsContext = new ProductDbContext();
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            string productName = txtName.Text;
            string productPrice = txtPrice.Text;

            using (var dbContext = new ProductDbContext())
                {
                    var product = new ProductModel { Name = productName, Price = productPrice };

                    dbContext.Products.Add(product);

                    dbContext.SaveChanges();
                }


                txtName.Text = "";
                txtPrice.Text = "";
        }
    }
}
