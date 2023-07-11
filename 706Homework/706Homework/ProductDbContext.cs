using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace _706Homework
{
    public class ProductDbContext : DbContext
    {
        public DbSet<ProductModel> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ConfigurationBuilder builder = new();

            builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            string connectionString = configuration.GetConnectionString("LocalConnection");

            optionsBuilder.UseSqlServer(connectionString);
        }

        protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            var country = modelBuilder.Entity<ProductModel>();

            country.HasKey(x => x.Id);
            country.Property(x => x.Name).IsRequired();
            country.Property(x => x.Price).IsRequired();
        }
    }
}
